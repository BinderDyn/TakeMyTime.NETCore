using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeMyTime.DOM.Interfaces
{
    /// <summary>
    /// Contains basic data of an entity a ViewModel shouldn't change.
    /// Architecture taken from: https://cpratt.co/generic-entity-base-class/
    /// </summary>
    public interface IEntity : IModifiableEntity
    {
        int Id { get; }
        DateTime Created { get; set; }
        DateTime? Edited { get; set; }
        //byte[] Version { get; set; }
    }

    /// <summary>
    /// Provides the Interface IEntity specified for Type and demanded Id-Type
    /// </summary>
    /// <typeparam name="T">Can be specified later on (GUID, Id, long)</typeparam>
    public interface IEntity<T> : IEntity
    {
    }
}
