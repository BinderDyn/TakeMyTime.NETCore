using BinderDyn.LoggingUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using TakeMyTime.Biz.ViewModels;
using TakeMyTime.DAL.uow;
using TakeMyTime.DOM.Models;
using Common.Enums;

namespace TakeMyTime.BLL.Logic
{
    public class XmlLogic
    {
        public void BackUpDatabaseToXml(IProgress<BackUpProgess> progress, string path)
        {
            Logger.Log(this.GetType().FullName + ".BackUpDatabaseToXml");
            Logger.Log(this.GetType().FullName + ".BackUpDatabaseToXml: Backup Projects");

            // Initiales XDocument erstellen

            var pl = new ProjectLogic();
            var projects = pl.GetAllProjects().ToArray();
            var el = new EntryLogic();
            var entries = el.GetAllEntries().Where(e => e.Assignment == null).ToList();
            var doc = BackUpEntities(projects, entries, progress);

            Logger.Log(this.GetType().FullName + ".BackUpDatabaseToXml: Writing to file");

            WriteToFile(path, doc);
            progress.Report(new BackUpProgess(100, "", "Done!"));
            Logger.Log(this.GetType().FullName + ".BackUpDatabaseToXml: Backup done");
        }

        [Todo]
        public void RestoreDatabaseFromXml(IProgress<RestoreProgress> progress, string filePath)
        {
            Logger.Log(this.GetType().FullName + ".RestoreDatabaseFromXml");
            Logger.Log(this.GetType().FullName + ".Deleting existing data...");
            DeleteDatabase(progress);
            Logger.Log(this.GetType().FullName + ".Database cleared");

            Logger.Log(this.GetType().FullName + ".RestoreDatabaseFromXml - Parsing file");
            var xdoc = XDocument.Load(filePath);

            Logger.Log(this.GetType().FullName + ".RestoreDatabaseFromXml - Processing...");
            ProcessProjects(xdoc, progress);
            Logger.Log(this.GetType().FullName + ".RestoreDatabaseFromXml - Processing complete, database restored");
        }

        #region Restore

        public void ProcessProjects(XDocument doc, IProgress<RestoreProgress> progress)
        {
            int entityCount = doc.Descendants().Count(d => d.Descendants().Count() > 0) - 1;
            int counter = 0;
            var XMLprojects = doc.Descendants("Project");
            var projectEntities = new List<Project>();
            var projects = new ProjectLogic().GetAllProjects();
            var existingEntities = GetAllExistingEntities(doc);

            foreach (var p in XMLprojects)
            {
                Project proj = null;
                var existingAssignments = GetAllExistingAssignmentsInProject(p);
                var existingEntries = GetAllExistingEntriesInProject(p);

                string projectName = p.Descendants("Name").FirstOrDefault().Value;

                var assignmentNames = p.Descendants("Assignment")
                    .SelectMany(a => a.Descendants("Name")
                    .Select(d => d.Value))
                    .ToList();

                var entryNames = p.Descendants("Entry")
                    .SelectMany(a => a.Descendants("Name")
                    .Select(d => d.Value))
                    .ToList();

                int projectId = 0;
                bool parsableId = Int32.TryParse(p.Descendants("Id").FirstOrDefault().Value, out projectId);
                if (!parsableId) throw new FormatException("Invalid value Id, not a number!");

                counter++;
                progress.Report(new RestoreProgress(CalculateProgress(entityCount, counter),
                    p.Descendants("Name").FirstOrDefault().Value,
                    string.Format("Restoring project {0}", p.Descendants("Name").FirstOrDefault().Value)));

                Project project = new Project
                {
                    Created = ParseDate(p.Descendants("Created").FirstOrDefault().Value),
                    Description = p.Descendants("Description").FirstOrDefault().Value,
                    Edited = ParseNullableDate(p.Descendants("Edited").FirstOrDefault().Value),
                    Name = projectName,
                    ProjectStatus = ParseProjectStatus(p.Descendants("ProjectStatus").FirstOrDefault().Value),
                    // ProjectType = ParseProjectType(p.Descendants("ProjectType").FirstOrDefault().Value),
                    WorkingTimeAsTicks = ParseNullableLong(p.Descendants("WorkingTimeAsTicks").FirstOrDefault().Value),
                    Assignments = new HashSet<Assignment>(),
                    Entries = new HashSet<Entry>()
                };

                var assignments = p.Descendants("Assignment");
                foreach (var a in assignments)
                {
                    counter++;
                    Assignment assignment = RestoreAssignment(progress, a, project, ref counter, entityCount);
                    project.Assignments.Add(assignment);
                }

                var entriesWithoutAssignment = p.Descendants("NotAssignedEntries");
                foreach (var e in entriesWithoutAssignment)
                {
                    counter++;
                    Entry entry = RestoreEntry(progress, e, project, null, ref counter, entityCount);
                    project.Entries.Add(entry);
                }

                proj = project;

                projectEntities.Add(project);

            }

            var projectLogic = new ProjectLogic();
            // projectLogic.InsertProjects(projectEntities);
        }

        private Assignment RestoreAssignment(IProgress<RestoreProgress> progress, XElement assignmentXml, Project project, ref int counter, int total)
        {
            var assignment = new Assignment
            {
                AssignmentStatus = ParseAssignmentStatus(assignmentXml.Descendants("AssignmentStatus").FirstOrDefault().Value),
                Description = assignmentXml.Descendants("Comment").FirstOrDefault().Value,
                Created = DateTime.Parse(assignmentXml.Descendants("Created").FirstOrDefault().Value),
                DateDue = DateTime.Parse(assignmentXml.Descendants("DateDue").FirstOrDefault().Value),
                DatePlanned = DateTime.Parse(assignmentXml.Descendants("DatePlanned").FirstOrDefault().Value),
                Name = assignmentXml.Descendants("Name").FirstOrDefault().Value,
                Edited = DateTime.Now,
                Entries = new HashSet<Entry>()
            };
            assignment.Project = project;
            // assignment.Pages =  ParseNullableInt(assignmentXml.Descendants("Pages").FirstOrDefault().Value);
            assignment.TimesPushed = ParseInt(assignmentXml.Descendants("TimesPushed").FirstOrDefault().Value);
            // assignment.Words = ParseInt(assignmentXml.Descendants("Words").FirstOrDefault().Value);
            // assignment.ProjectId = Int32.TryParse(assignmentXml.Descendants("ProjectId").FirstOrDefault().Value, out int projectId) ? projectId : 0;
            // assignment.UserId = null; // Int32.TryParse(assignmentXml.Descendants("UserId").FirstOrDefault().Value, out int userId) ? userId : 0;
            assignment.DurationPlannedAsTicks = ParseNullableLong(assignmentXml.Descendants("DurationPlannedAsTicks").FirstOrDefault().Value);


            progress.Report(new RestoreProgress(CalculateProgress(total, counter),
                assignment.Name,
                string.Format("Restoring assignment: {0}", assignment.Name)));

            bool hasEntries = assignmentXml.Descendants("Entry").Any();
            if (hasEntries)
            {
                var entries = assignmentXml.Descendants("Entry").ToList();
                foreach (var e in entries)
                {
                    counter++;
                    var entry = RestoreEntry(progress, e, project, assignment, ref counter, total);
                    assignment.Entries.Add(entry);
                }
            }

            return assignment;
        }

        private Entry RestoreEntry(IProgress<RestoreProgress> progress, XElement entryXml, Project project, Assignment assignment, ref int counter, int total)
        {
            Entry entry = new Entry
            {
                Comment = entryXml.Descendants("Comment").FirstOrDefault().Value,
                DurationAsTicks = long.Parse(entryXml.Descendants("DurationAsTicks").FirstOrDefault().Value),
                Name = entryXml.Descendants("Name").FirstOrDefault().Value,
                Project = project
            };

            entry.Ended = ParseNullableDate(entryXml.Descendants("Ended").FirstOrDefault().Value);
            entry.Started = ParseNullableDate(entryXml.Descendants("Started").FirstOrDefault().Value);
            entry.Edited = ParseNullableDate(entryXml.Descendants("Edited").FirstOrDefault().Value);
            // entry.Date = ParseNullableDate(entryXml.Descendants("Date").FirstOrDefault().Value);
            entry.Created = ParseDate(entryXml.Descendants("Created").FirstOrDefault().Value);
            // entry.Pages = ParseNullableInt(entryXml.Descendants("Pages").FirstOrDefault().Value);
            // entry.Words = ParseNullableInt(entryXml.Descendants("Words").FirstOrDefault().Value);

            if (assignment != null)
            {
                entry.Assignment = assignment;
            }

            progress.Report(new RestoreProgress(CalculateProgress(total, counter), entry.Name, string.Format("Restoring entry: {0}", entry.Name)));

            return entry;
        }

        #endregion

        #region Backup

        public XDocument BackUpEntities(Project[] projects, List<Entry> entries, IProgress<BackUpProgess> progress)
        {
            var doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"));
            doc.AddFirst(new XElement("Backup"));
            var mainNode = doc.Descendants("Backup").Single();
            mainNode.SetAttributeValue("Date", DateTime.Now);
            int entitiesToProc = projects.Count() + projects.Sum(p => p.Assignments.Count) + projects.Sum(p => p.Assignments.Sum(a => a.Entries.Count));
            entitiesToProc += entries.Count();
            int entityCount = 0;

            foreach (Project p in projects)
            {
                entityCount++;
                var processedProject = ProcessEntity(p);

                progress.Report(new BackUpProgess(CalculateProgress(entitiesToProc, entityCount), p.Name, string.Format("Processing project {0}", p.Name)));

                foreach (Assignment a in p.Assignments)
                {
                    entityCount++;
                    var processedAssignment = ProcessEntity(a);

                    progress.Report(new BackUpProgess(CalculateProgress(entitiesToProc, entityCount), a.Name, string.Format("Processing assignments for project {0}", p.Name)));

                    processedProject.Add(processedAssignment);

                    foreach (Entry e in a.Entries)
                    {
                        entityCount++;
                        var processedEntry = ProcessEntity(e);

                        progress.Report(new BackUpProgess(CalculateProgress(entitiesToProc, entityCount), e.Name, string.Format("Processing entries for assignment {0}", a.Name)));

                        processedAssignment.Add(processedEntry);
                    }
                }

                mainNode.Add(processedProject);

                // Verarbeiten aller Entries, die keine AssignmentId haben
                if (entries.Count(e => e.Project_Id == p.Id) > 0)
                {
                    processedProject.Add(new XElement("NotAssignedEntries"));
                    var entriesOfProject = processedProject.Descendants("NotAssignedEntries").Single();

                    foreach (Entry notAssignedEntry in entries.Where(e => e.Project_Id == p.Id))
                    {
                        entityCount++;
                        var processedNotAssigned = ProcessEntity(notAssignedEntry);

                        progress.Report(new BackUpProgess(CalculateProgress(entitiesToProc, entityCount), notAssignedEntry.Name, "Proccesing entries without assignment"));

                        entriesOfProject.Add(processedNotAssigned);
                    }
                }
            }

            progress.Report(new BackUpProgess(99, "", "Writing to file..."));
            
            return doc;
        }

        private XElement ProcessEntity(object entity)
        {
            var props = entity.GetType().GetProperties();

            var elem = new XElement(GetEntityClassName(entity));

            Func<PropertyInfo, bool> predicate = WhereNotCollectionOrNavigationProperty;

            foreach (var p in props.Where(predicate))
            {
                XElement propXElem = null;

                propXElem = new XElement(p.Name, p.GetValue(entity));


                if (p.GetCustomAttribute<KeyAttribute>() != null)
                {
                    propXElem.SetAttributeValue("Key", p.GetValue(entity));
                }

                else if (p.GetCustomAttribute<ForeignKeyAttribute>() != null)
                {
                    propXElem.SetAttributeValue("ForeignKey", p.GetValue(entity));
                }

                elem.Add(propXElem);
            }

            return elem;
        }

        #endregion

        #region Utility

        private int CalculateProgress(int total, decimal current)
        {
            return (int)Math.Round((current / total) * 100, 0);
        }

        private void WriteToFile(string path, XDocument doc)
        {
            doc.Save(path, SaveOptions.OmitDuplicateNamespaces);
        }

        private string GetEntityClassName(object entity)
        {
            var fullName = entity.GetType().FullName;
            string[] cutName = fullName.Replace("System.Data.Entity.DynamicProxies.", string.Empty).Split('_');
            return cutName[0];
        }

        private static bool WhereNotCollectionOrNavigationProperty(PropertyInfo value)
        {
            return value.Name != "Assignment" && value.Name != "Project" && value.Name != "User" &&
                   value.Name != "Assignments" && value.Name != "Entries" && value.Name != "Users";
        }

        private int CountAlreadyInDatabase(XDocument doc)
        {
            return GetAllExistingEntities(doc).Count;
        }

        private List<EntityCollectionItem> GetAllExistingEntities(XDocument doc)
        {
            var list = new List<EntityCollectionItem>();
            var entitiesInXml = doc.Descendants("Backup").Single().Descendants().Where(d => d.Descendants().Count() > 0);
            var projects = new ProjectLogic().GetAllProjects();
            var assignments = projects.SelectMany(p => p.Assignments);
            var entries = projects.SelectMany(p => p.Entries);

            foreach (var e in entitiesInXml)
            {
                string name = e.Descendants("Name").FirstOrDefault().Value;
                int id = Int32.Parse(e.Descendants("Id").FirstOrDefault().Value);

                if (projects.Count(p => p.Name == name && p.Id == id || p.Name == name) > 0)
                {
                    list.Add(new EntityCollectionItem(id, EnumDefinition.BackupEntityType.Project, name));
                }
                else if (assignments.Count(a => a.Name == name && a.Id == id || a.Name == name) > 0)
                {
                    list.Add(new EntityCollectionItem(id, EnumDefinition.BackupEntityType.Assignment, name));
                }
                else if (entries.Count(en => en.Name == name && en.Id == id || en.Name == name) > 0)
                {
                    list.Add(new EntityCollectionItem(id, EnumDefinition.BackupEntityType.Entry, name));
                }
            }

            return list;
        }

        private List<EntityCollectionItem> GetAllExistingAssignmentsInProject(XElement project)
        {
            var list = new List<EntityCollectionItem>();
            var entitiesInXml = project.Descendants("Assignment");
            var assignments = new AssignmentLogic().GetAllAssignments();

            foreach (var a in entitiesInXml)
            {
                string name = a.Descendants("Name").FirstOrDefault().Value;
                int id = Int32.Parse(entitiesInXml.Descendants("Id").FirstOrDefault().Value);

                if (assignments.Count(assignment => assignment.Name == name && assignment.Id == id || assignment.Name == name) > 0)
                {
                    list.Add(new EntityCollectionItem(id, EnumDefinition.BackupEntityType.Assignment, name));
                }
            }

            return list;
        }

        private List<EntityCollectionItem> GetAllExistingEntriesInProject(XElement project)
        {
            var list = new List<EntityCollectionItem>();
            var entitiesInXml = project.Descendants("Entry");
            var entries = new EntryLogic().GetAllEntries();

            foreach (var e in entitiesInXml)
            {
                string name = e.Descendants("Name").FirstOrDefault().Value;
                int id = Int32.Parse(entitiesInXml.Descendants("Id").FirstOrDefault().Value);

                if (entries.Count(entry => entry.Name == name && entry.Id == id || entry.Name == name) > 0)
                {
                    list.Add(new EntityCollectionItem(id, EnumDefinition.BackupEntityType.Entry, name));
                }
            }

            return list;
        }



        private static EnumDefinition.ProjectStatus ParseProjectStatus(string status)
        {
            switch (status)
            {
                case "Active":
                    return EnumDefinition.ProjectStatus.Active;
                case "Archived":
                    return EnumDefinition.ProjectStatus.Archived;
                default:
                    return EnumDefinition.ProjectStatus.Active;
            }
        }

        //private static EnumDefinition.ProjectType ParseProjectType(string type)
        //{
        //    switch (type)
        //    {
        //        case "Standard":
        //            return EnumDefinition.ProjectType.Default;
        //        case "Writing":
        //            return EnumDefinition.ProjectType.Book;
        //        case "Language":
        //            return EnumDefinition.ProjectType.Language;
        //        case "Programming":
        //            return EnumDefinition.ProjectType.Programming;
        //        default:
        //            return EnumDefinition.ProjectType.Default;
        //    }
        //}

        private static EnumDefinition.AssignmentStatus ParseAssignmentStatus(string status)
        {
            switch (status)
            {
                case "All":
                    return EnumDefinition.AssignmentStatus.Default;
                case "Active":
                    return EnumDefinition.AssignmentStatus.InProgress;
                case "Future":
                    return EnumDefinition.AssignmentStatus.Future;
                case "Done":
                    return EnumDefinition.AssignmentStatus.Done;
                case "Aborted":
                    return EnumDefinition.AssignmentStatus.Aborted;
                case "Postponed":
                    return EnumDefinition.AssignmentStatus.Postponed;
                default:
                    return EnumDefinition.AssignmentStatus.Default;
            }
        }

        private static int ParseInt(string value)
        {
            int result;

            if(Int32.TryParse(value, out result))
            {
                return result;
            }
            return 0;
        }

        private static int? ParseNullableInt(string value)
        {
            int result;

            if (Int32.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        private static DateTime? ParseNullableDate(string value)
        {
            DateTime date;
            
            if(DateTime.TryParse(value, out date))
            {
                return date;
            }
            return null;
        }

        private static DateTime ParseDate(string value)
        {
            DateTime date;

            if (DateTime.TryParse(value, out date))
            {
                return date;
            }
            return DateTime.Now;
        }

        private static long? ParseNullableLong(string value)
        {
            long ticks;

            if(long.TryParse(value, out ticks))
            {
                return ticks;
            }
            return null;
        }

        public void DeleteDatabase(IProgress<RestoreProgress> progress)
        {
            progress.Report(new RestoreProgress(0, null, "Fetching entries"));
            var entries = new EntryLogic().GetAllEntries();
            progress.Report(new RestoreProgress(0, null, "Fetching assignments"));
            var assignments = new AssignmentLogic().GetAllAssignments();
            progress.Report(new RestoreProgress(0, null, "Fetching projects"));
            var projects = new ProjectLogic().GetAllProjects();
            UnitOfWork uow = new UnitOfWork();

            var total = (entries.Count() + assignments.Count() + projects.Count());
            if (total == 0) return;

            int progressCount;

            DeleteEntries(entries);
            progressCount = entries.Count() / total * 100;
            progress.Report(new RestoreProgress(progressCount, null, "Deleted entries"));
            DeleteAssignments(assignments);
            progressCount = (assignments.Count() + entries.Count()) / total * 100;
            progress.Report(new RestoreProgress(progressCount, null, "Deleted assignments"));
            DeleteProjects(projects);
            progress.Report(new RestoreProgress(total / total * 100, null, "Deleted projects"));
        }

        private void DeleteEntries(IEnumerable<Entry> entries)
        {
            var el = new EntryLogic();
            el.DeleteEntries(entries);
            el.Dispose();
        }

        private void DeleteAssignments(IEnumerable<Assignment> assignments)
        {
            var al = new AssignmentLogic();
            al.DeleteAssignments(assignments);
            al.Dispose();
        }

        private void DeleteProjects(IEnumerable<Project> projects)
        {
            var pl = new ProjectLogic();
            pl.DeleteProjects(projects);
            pl.Dispose();
        }

        #endregion
    }
}
