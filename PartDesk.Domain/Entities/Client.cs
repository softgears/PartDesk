// 
// 
// Solution: PartDesk
// Project: PartDesk.Domain
// File: Client.cs
// 
// Created by: ykors_000 at 19.11.2013 11:10
// 
// Property of SoftGears
// 
// ========

using System;
using PartDesk.Domain.Utils;

namespace PartDesk.Domain.Entities
{
    /// <summary>
    /// Клиент, заказывающий запчасть
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2} ({3})", LastName, FirstName, SurName,Phone.FormatPhoneNumber());
        }
    }
}