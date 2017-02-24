﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicORM
{
    public interface ITable
    {
        /// <summary>
        /// Save the object 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        bool Save(ITable table);

        /// <summary>
        /// Get all the data 
        /// </summary>
        /// <returns></returns>
        IEnumerable<ITable> GetAll();

        /// <summary>
        /// Get an item based on a given ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        ITable Get(int ID);

        /// <summary>
        /// Updates the item
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        bool Update(ITable table);

        /// <summary>
        /// Remove the item
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        bool Remove(ITable table);

        /// <summary>
        /// Gets the last ID
        /// </summary>
        /// <returns></returns>
        int GetLastID();
    }
}