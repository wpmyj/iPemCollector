﻿using System;

namespace iPem.Data.Common {
    public static class SqlCommands_Cs {
        //ActAlm Repository
        public const string Sql_ActAlm_Repository_GetEntities1 = @"SELECT * FROM [dbo].[A_Act] WHERE [StartTime] BETWEEN @Start AND @End ORDER BY [StartTime] DESC;";
        public const string Sql_ActAlm_Repository_GetEntities2 = @"SELECT * FROM [dbo].[A_Act] ORDER BY [StartTime] DESC;";
        //HisAlm Repository
        public const string Sql_HisAlm_Repository_GetEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[A_Hist'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisAlm AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT * FROM HisAlm;'
        END

        EXECUTE sp_executesql @SQL;";
        //HisBat Repository
        public const string Sql_HisBat_Repository_GetEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[V_Bat'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [ValueTime] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisBat AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT * FROM HisBat ORDER BY [ValueTime];'
        END

        EXECUTE sp_executesql @SQL;";
        //HisBatTime Repository
        public const string Sql_HisBatTime_Repository_SaveEntities = @"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'TT_BatTime{0}') AND type in (N'U'))
        BEGIN
            CREATE TABLE [dbo].[TT_BatTime{0}](
	            [DeviceId] [varchar](100) NOT NULL,
	            [Period] [datetime] NOT NULL,
	            [Value] [float] NOT NULL,
	            [CreatedTime] [datetime] NOT NULL,
                CONSTRAINT [PK_TT_BatTime{0}] PRIMARY KEY CLUSTERED 
            (
	            [DeviceId] ASC,
	            [Period] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
        END
        UPDATE [dbo].[TT_BatTime{0}] SET [Value] = @Value WHERE [DeviceId] = @DeviceId AND [Period] = @Period;
        IF(@@ROWCOUNT = 0)
        BEGIN
	        INSERT INTO [dbo].[TT_BatTime{0}]([DeviceId],[Period],[Value],[CreatedTime]) VALUES(@DeviceId,@Period,@Value,@CreatedTime);
        END";
        public const string Sql_HisBatTime_Repository_DeleteEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TT_BatTime'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                SET @SQL += N'
		        DELETE FROM ' + @tbName + N' WHERE [Period] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N''';';
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        EXECUTE sp_executesql @SQL;";
        //HisElec Repository
        public const string Sql_HisElec_Repository_SaveEntities = @"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'TT_Elec{0}') AND type in (N'U'))
        BEGIN
        CREATE TABLE [dbo].[TT_Elec{0}](
	        [Id] [varchar](100) NOT NULL,
	        [Type] [int] NOT NULL,
	        [FormulaType] [int] NOT NULL,
	        [Period] [datetime] NOT NULL,
	        [Value] [float] NOT NULL,
         CONSTRAINT [PK_TT_Elec{0}] PRIMARY KEY CLUSTERED 
        (
	        [Id] ASC,
	        [Type] ASC,
	        [FormulaType] ASC,
	        [Period] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        END
        UPDATE [dbo].[TT_Elec{0}] SET [Value] = @Value WHERE [Id] = @Id AND [Type] = @Type AND [FormulaType] = @FormulaType AND [Period] = @Period;
        IF(@@ROWCOUNT = 0)
        BEGIN
	        INSERT INTO [dbo].[TT_Elec{0}]([Id],[Type],[FormulaType],[Period],[Value]) VALUES(@Id,@Type,@FormulaType,@Period,@Value);
        END";
        public const string Sql_HisElec_Repository_DeleteEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TT_Elec'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                SET @SQL += N'
		        DELETE FROM ' + @tbName + N' WHERE [Period] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N''';';
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        EXECUTE sp_executesql @SQL;";
        //HisLoadRate Repository
        public const string Sql_HisLoadRate_Repository_SaveEntities = @"
        IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'TT_LoadRate{0}') AND type in (N'U'))
        BEGIN
        CREATE TABLE [dbo].[TT_LoadRate{0}](
	        [DeviceId] [varchar](100) NOT NULL,
	        [Period] [datetime] NOT NULL,
	        [Value] [float] NOT NULL,
	        [CreatedTime] [datetime] NOT NULL,
         CONSTRAINT [PK_TT_LoadRate{0}] PRIMARY KEY CLUSTERED 
        (
	        [DeviceId] ASC,
	        [Period] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY]
        END
        UPDATE [dbo].[TT_LoadRate{0}] SET [Value] = @Value WHERE [DeviceId] = @DeviceId AND [Period] = @Period;
        IF(@@ROWCOUNT = 0)
        BEGIN
	        INSERT INTO [dbo].[TT_LoadRate{0}]([DeviceId],[Period],[Value],[CreatedTime]) VALUES(@DeviceId,@Period,@Value,@CreatedTime);
        END";
        public const string Sql_HisLoadRate_Repository_DeleteEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[TT_LoadRate'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                SET @SQL += N'
		        DELETE FROM ' + @tbName + N' WHERE [Period] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N''';';
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END
        EXECUTE sp_executesql @SQL;";
        //HisStatic Repository
        public const string Sql_HisStatic_Repository_GetEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[V_Static'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [BeginTime] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisStatic AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT * FROM HisStatic ORDER BY [BeginTime];'
        END

        EXECUTE sp_executesql @SQL;";
        //HisValue Repository
        public const string Sql_HisValue_Repository_GetEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[V_Hist'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [Time] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisValue AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT * FROM HisValue ORDER BY [Time];'
        END

        EXECUTE sp_executesql @SQL;";
        public const string Sql_HisValue_Repository_GetEntitiesByPoint = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[V_Hist'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [DeviceId] = ' + @DeviceId + N' AND [PointId] = ' + @PointId + N' AND [Time] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisValue AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT * FROM HisValue ORDER BY [Time];'
        END

        EXECUTE sp_executesql @SQL;";
        public const string Sql_HisValue_Repository_GetProcedure = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[V_Hist'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [DeviceId] = ' + @DeviceId + N' AND [PointId] = ' + @PointId + N' AND [Time] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH HisValue AS
		        (
			        ' + @SQL + N'
		        )
		        SELECT MAX([Value]) AS [MaxValue], MIN([Value]) AS [MinValue] FROM HisValue;'
        END

        EXECUTE sp_executesql @SQL;";
    }
}