﻿using System;

namespace iPem.Core {
    [Serializable]
    public partial class HisStatic {
        /// <summary>
        /// Gets or sets the device identifier
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the point identifier
        /// </summary>
        public string PointId { get; set; }

        /// <summary>
        /// Gets or sets the begintime
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets or sets the endtime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public double MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public double AvgValue { get; set; }

        /// <summary>
        /// Gets or sets the datetime
        /// </summary>
        public DateTime MaxTime { get; set; }

        /// <summary>
        /// Gets or sets the datetime
        /// </summary>
        public DateTime MinTime { get; set; }

        /// <summary>
        /// Gets or sets the total
        /// </summary>
        public int Total { get; set; }
    }
}