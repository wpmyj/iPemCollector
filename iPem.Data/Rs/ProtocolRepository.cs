﻿using iPem.Core;
using iPem.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iPem.Data {
    public partial class ProtocolRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ProtocolRepository() {
            this._databaseConnectionString = SqlHelper.ConnectionStringRsTransaction;
        }

        #endregion

        #region Methods

        public List<Protocol> GetEntities() {
            var entities = new List<Protocol>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_Protocol_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Protocol();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.SubDeviceType = new SubDeviceType { Id = SqlTypeConverter.DBNullStringHandler(rdr["SubDeviceTypeId"]), Name = SqlTypeConverter.DBNullStringHandler(rdr["SubDeviceTypeName"]) };
                    entity.DeviceType = new DeviceType { Id = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeId"]), Name = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeName"]) };
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}
