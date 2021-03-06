﻿using iPem.Core;
using iPem.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iPem.Data {
    public partial class H_FsuEventRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public H_FsuEventRepository() {
            this._databaseConnectionString = SqlHelper.ConnectionStringCsTransaction;
        }

        #endregion

        #region Methods

        public List<H_FsuEvent> GetEntities(DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };

            parms[0].Value = SqlTypeConverter.DBNullDateTimeHandler(start);
            parms[1].Value = SqlTypeConverter.DBNullDateTimeHandler(end);

            var entities = new List<H_FsuEvent>();
            using (var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cs.Sql_H_FsuEvent_Repository_GetEntities1, parms)) {
                while (rdr.Read()) {
                    var entity = new H_FsuEvent();
                    entity.FsuId = SqlTypeConverter.DBNullStringHandler(rdr["FsuId"]);
                    entity.EventType = SqlTypeConverter.DBNullEnmFtpEventHandler(rdr["EventType"]);
                    entity.EventDesc = SqlTypeConverter.DBNullStringHandler(rdr["EventDesc"]);
                    entity.EventTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["EventTime"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<H_FsuEvent> GetEntities(DateTime start, DateTime end, EnmFtpEvent type) {
            SqlParameter[] parms = { new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime),
                                     new SqlParameter("@EventType", SqlDbType.Int) };

            parms[0].Value = SqlTypeConverter.DBNullDateTimeHandler(start);
            parms[1].Value = SqlTypeConverter.DBNullDateTimeHandler(end);
            parms[2].Value = (int)type;

            var entities = new List<H_FsuEvent>();
            using (var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cs.Sql_H_FsuEvent_Repository_GetEntities2, parms)) {
                while (rdr.Read()) {
                    var entity = new H_FsuEvent();
                    entity.FsuId = SqlTypeConverter.DBNullStringHandler(rdr["FsuId"]);
                    entity.EventType = SqlTypeConverter.DBNullEnmFtpEventHandler(rdr["EventType"]);
                    entity.EventDesc = SqlTypeConverter.DBNullStringHandler(rdr["EventDesc"]);
                    entity.EventTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["EventTime"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}
