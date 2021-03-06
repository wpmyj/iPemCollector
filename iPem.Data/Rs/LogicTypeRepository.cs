﻿using iPem.Core;
using iPem.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iPem.Data {
    public partial class LogicTypeRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        public LogicTypeRepository() {
            this._databaseConnectionString = SqlHelper.ConnectionStringRsTransaction;
        }

        #endregion

        #region Methods

        public LogicType GetEntity(string id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.VarChar, 100) };
            parms[0].Value = SqlTypeConverter.DBNullStringChecker(id);

            LogicType entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_LogicType_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new LogicType();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.DeviceTypeId = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeId"]);
                }
            }
            return entity;
        }

        public SubLogicType GetSubEntity(string id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.VarChar, 100) };
            parms[0].Value = SqlTypeConverter.DBNullStringChecker(id);

            SubLogicType entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_LogicType_Repository_GetSubEntity, parms)) {
                if(rdr.Read()) {
                    entity = new SubLogicType();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.LogicTypeId = SqlTypeConverter.DBNullStringHandler(rdr["LogicTypeId"]);
                }
            }
            return entity;
        }

        public List<LogicType> GetEntities() {
            var entities = new List<LogicType>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_LogicType_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new LogicType();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.DeviceTypeId = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeId"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<SubLogicType> GetSubEntities(string parent) {
            SqlParameter[] parms = { new SqlParameter("@LogicTypeId", SqlDbType.VarChar, 100) };
            parms[0].Value = SqlTypeConverter.DBNullStringChecker(parent);

            var entities = new List<SubLogicType>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_LogicType_Repository_GetSubEntitiesByParent, parms)) {
                while(rdr.Read()) {
                    var entity = new SubLogicType();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.LogicTypeId = SqlTypeConverter.DBNullStringHandler(rdr["LogicTypeId"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<SubLogicType> GetSubEntities() {
            var entities = new List<SubLogicType>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Rs.Sql_LogicType_Repository_GetSubEntities, null)) {
                while(rdr.Read()) {
                    var entity = new SubLogicType();
                    entity.Id = SqlTypeConverter.DBNullStringHandler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.LogicTypeId = SqlTypeConverter.DBNullStringHandler(rdr["LogicTypeId"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}
