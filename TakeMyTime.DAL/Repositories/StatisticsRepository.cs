﻿using Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMyTime.DAL.Interfaces;
using TakeMyTime.DOM.Models;

namespace TakeMyTime.DAL.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        TakeMyTimeDbContext context;

        public StatisticsRepository(TakeMyTimeDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Tuple<int, string, double>> GetAssignmentSharesOfProject(int project_id)
        {
            var results = new List<Tuple<int, string, double>>();
            var assignmentsOfProject = this.context.Assignments
                .Include(a => a.Subtasks)
                .Where(a => a.Project_Id == project_id);
            var totalWorkingTime = GetTotalWorktimeOfSpecificProject(project_id);

            if (totalWorkingTime > 0)
            {
                foreach (var ass in assignmentsOfProject)
                {
                    var workingTimeForAssignment = GetWorkingTimeForAssignment(ass);
                    if (workingTimeForAssignment > 0)
                    {
                        double shareOfAssignment = Math.Round((double)(workingTimeForAssignment / totalWorkingTime * 100), 2);
                        results.Add(new Tuple<int, string, double>(ass.Id, ass.Name, shareOfAssignment));
                    }
                }
            }

            return results;
        }

        private long? GetWorkingTimeForAssignment(Assignment assignment)
        {
            var entriesForAssignment = this.context.Subtasks
                .Include(s => s.Entries)
                .Where(s => s.Assignment_Id == assignment.Id)
                .SelectMany(s => s.Entries);
            return entriesForAssignment.Sum(e => e.DurationAsTicks);
        }

        public long? GetTotalWorktimeOfAllActiveProjects()
        {
            var entriesFromActiveProjects = this.context.Entries
                .Include(e => e.Project)
                .Where(e => e.Project.ProjectStatus == EnumDefinition.ProjectStatus.Active);
            return entriesFromActiveProjects.Sum(e => e.DurationAsTicks);
        }

        public long? GetTotalWorktimeOfSpecificProject(int project_id)
        {
            return this.context.Entries
                .Where(e => e.Project_Id == project_id)
                .Sum(e => e.DurationAsTicks);
        }

        public IEnumerable<Tuple<int, string, double>> GetProjectTotalShares()
        {
            var results = new List<Tuple<int, string, double>>();
            var projects = this.context.Projects.Where(p => p.ProjectStatus == EnumDefinition.ProjectStatus.Active);
            var worktimeOfAllProjects = GetTotalWorktimeOfAllActiveProjects();
            if (worktimeOfAllProjects > 0)
            {
                foreach (var project in projects)
                {
                    var worktimeOfProject = GetTotalWorktimeOfSpecificProject(project.Id);
                    if (worktimeOfProject > 0)
                    {
                        double shareOfProject = Math.Round((double)(worktimeOfProject / worktimeOfAllProjects * 100), 2);
                        results.Add(new Tuple<int, string, double>(project.Id, project.Name, shareOfProject));
                    }
                }
            }
            return results;
        }
    }
}
