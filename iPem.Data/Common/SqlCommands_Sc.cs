﻿using System;

namespace iPem.Data.Common {
    public static class SqlCommands_Sc {
        /// <summary>
        /// Reservation Repository
        /// </summary>
        public const string Sql_Reservation_Repository_GetEntities1 = @"SELECT * FROM [dbo].[M_Reservations] ORDER BY [CreatedTime];";
        public const string Sql_Reservation_Repository_GetEntities2 = @"SELECT * FROM [dbo].[M_Reservations] WHERE NOT ([StartTime]>@endTime OR [EndTime]<@startTime) ORDER BY [CreatedTime];";
        public const string Sql_Reservation_Repository_GetEntities3 = @"SELECT * FROM [dbo].[M_Reservations] WHERE [CreatedTime] > @CreatedTime ORDER BY [CreatedTime];";
        
        /// <summary>
        /// Dictionary Repository
        /// </summary>
        public const string Sql_Dictionary_Repository_GetEntity = @"SELECT * FROM [dbo].[M_Dictionary] WHERE [Id]=@Id;";
        public const string Sql_Dictionary_Repository_GetEntities = @"SELECT * FROM [dbo].[M_Dictionary];";
        
        /// <summary>
        /// Formula Repository
        /// </summary>
        public const string Sql_Formula_Repository_GetEntity = @"SELECT * FROM [dbo].[M_Formulas] WHERE [Id]=@Id AND [Type]=@Type AND [FormulaType]=@FormulaType;";
        public const string Sql_Formula_Repository_GetEntities = @"SELECT * FROM [dbo].[M_Formulas] WHERE [Id]=@Id AND [Type]=@Type;";
        public const string Sql_Formula_Repository_GetAllEntities = @"SELECT * FROM [dbo].[M_Formulas];";
        
        /// <summary>
        /// NodesInReservation Repository
        /// </summary>
        public const string Sql_NodesInReservation_Repository_GetEntities = @"SELECT * FROM [dbo].[M_NodesInReservation];";
        public const string Sql_NodesInReservation_Repository_GetEntitiesByNodeType = @"SELECT * FROM [dbo].[M_NodesInReservation] WHERE [NodeType]=@NodeType;";
        public const string Sql_NodesInReservation_Repository_GetEntitiesById = @"SELECT * FROM [dbo].[M_NodesInReservation] WHERE [ReservationId]=@ReservationId;";
        
        /// <summary>
        /// Project Repository
        /// </summary>
        public const string Sql_Project_Repository_GetEntity = @"SELECT * FROM [dbo].[M_Projects] WHERE [Id]=@Id;";        
        public const string Sql_Project_Repository_GetEntities = @"SELECT * FROM [dbo].[M_Projects] ORDER BY [CreatedTime];";
        public const string Sql_Project_Repository_GetEntitiesByDate = @"SELECT * FROM [dbo].[M_Projects] WHERE [StartTime]>=@starttime AND [EndTime]<=@endtime ORDER BY [Name];";
    }
}
