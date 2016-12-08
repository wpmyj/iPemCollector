﻿using iPem.Core;
using iPem.Data.Common;
using iPem.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace iPem.Data {
    public partial class HisValueRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public HisValueRepository() {
            this._databaseConnectionString = SqlHelper.ConnectionStringCsTransaction;
        }

        #endregion

        #region Methods

        public List<HisValue> GetEntities(DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };

            parms[0].Value = SqlTypeConverter.DBNullDateTimeHandler(start);
            parms[1].Value = SqlTypeConverter.DBNullDateTimeHandler(end);

            var entities = new List<HisValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cs.Sql_HisValue_Repository_GetEntities, parms)) {
                while(rdr.Read()) {
                    var entity = new HisValue();
                    entity.DeviceId = SqlTypeConverter.DBNullStringHandler(rdr["DeviceId"]);
                    entity.PointId = SqlTypeConverter.DBNullStringHandler(rdr["PointId"]);
                    entity.ValType = SqlTypeConverter.DBNullEnmValTypeHandler(rdr["Type"]);
                    entity.Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]);
                    entity.Threshold = SqlTypeConverter.DBNullDoubleHandler(rdr["Threshold"]);
                    entity.State = SqlTypeConverter.DBNullEnmStateHandler(rdr["State"]);
                    entity.Time = SqlTypeConverter.DBNullDateTimeHandler(rdr["Time"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<HisValue> GetEntities(string device, string point, DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@DeviceId", SqlDbType.VarChar, 100),
                                     new SqlParameter("@PointId", SqlDbType.VarChar, 100),
                                     new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };

            parms[0].Value = SqlTypeConverter.DBNullStringChecker(device);
            parms[1].Value = SqlTypeConverter.DBNullStringChecker(point);
            parms[2].Value = SqlTypeConverter.DBNullDateTimeHandler(start);
            parms[3].Value = SqlTypeConverter.DBNullDateTimeHandler(end);

            var entities = new List<HisValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cs.Sql_HisValue_Repository_GetEntitiesByPoint, parms)) {
                while(rdr.Read()) {
                    var entity = new HisValue();
                    entity.DeviceId = SqlTypeConverter.DBNullStringHandler(rdr["DeviceId"]);
                    entity.PointId = SqlTypeConverter.DBNullStringHandler(rdr["PointId"]);
                    entity.ValType = SqlTypeConverter.DBNullEnmValTypeHandler(rdr["Type"]);
                    entity.Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]);
                    entity.Threshold = SqlTypeConverter.DBNullDoubleHandler(rdr["Threshold"]);
                    entity.State = SqlTypeConverter.DBNullEnmStateHandler(rdr["State"]);
                    entity.Time = SqlTypeConverter.DBNullDateTimeHandler(rdr["Time"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public double GetValDiff(string device, string point, DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@DeviceId", SqlDbType.VarChar, 100),
                                     new SqlParameter("@PointId", SqlDbType.VarChar, 100),
                                     new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };

            parms[0].Value = SqlTypeConverter.DBNullStringChecker(device);
            parms[1].Value = SqlTypeConverter.DBNullStringChecker(point);
            parms[2].Value = SqlTypeConverter.DBNullDateTimeHandler(start);
            parms[3].Value = SqlTypeConverter.DBNullDateTimeHandler(end);

            var value = 0d;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cs.Sql_HisValue_Repository_GetProcedure, parms)) {
                if(rdr.Read()) {
                    var max = SqlTypeConverter.DBNullDoubleHandler(rdr["MaxValue"]);
                    var min = SqlTypeConverter.DBNullDoubleHandler(rdr["MinValue"]);
                    value = max - min;
                }
            }
            return value;
        }

        #endregion

    }
}