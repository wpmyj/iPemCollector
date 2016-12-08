﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace iPem.Core {
    public partial class CommonHelper {
        public static string GlobalSeparator {
            get { return "┆"; }
        }

        public static List<object> GetIntervalStore() {
            var data = new List<object>();
            data.Add(new { Id = 3600, Name = "小时/次" });
            data.Add(new { Id = 60, Name = "分钟/次" });
            data.Add(new { Id = 1, Name = "秒钟/次" });
            return data;
        }

        public static List<object> GetDbTypeStore() {
            var data = new List<object>();
            foreach(DatabaseType dbType in Enum.GetValues(typeof(DatabaseType))) {
                data.Add(new { Id = (int)dbType, Name = dbType.ToString() });
            }
            return data;
        }

        public static string ToDateString(DateTime current) {
            if(current == default(DateTime)) return string.Empty;

            return current.ToString("yyyy-MM-dd");
        }

        public static string ToTimeString(DateTime current) {
            if(current == default(DateTime)) return string.Empty;

            return current.ToString("HH:mm:ss");
        }

        public static string ToDateTimeString(DateTime current) {
            if(current == default(DateTime)) return string.Empty;

            return current.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string[] SplitKeys(string key) {
            if(string.IsNullOrWhiteSpace(key))
                return new string[] { };

            return key.Split(new string[] { GlobalSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string JoinKeys(params string[] keys) {
            if(keys == null || keys.Length == 0)
                return string.Empty;

            return string.Join(GlobalSeparator, keys);
        }

        public static bool ValidateFormula(string formula) {
            if(string.IsNullOrWhiteSpace(formula)) return false;
            formula = Regex.Replace(formula, @"\s+", "");

            if(Regex.IsMatch(formula, @"[\+\-\*\/]{2,}")) return false;
            if(Regex.IsMatch(formula, @"[\+\-\*\/]{2,}")) return false;

            var stack = new Stack<char>();
            foreach(var letter in formula) {
                if(letter == '(') {
                    stack.Push('(');
                } else if(letter == ')') {
                    if(stack.Count == 0) return false;
                    stack.Pop();
                }
            }

            if(stack.Count != 0) return false;
            if(Regex.IsMatch(formula, @"\([\+\-\*\/]")) return false;
            if(Regex.IsMatch(formula, @"[\+\-\*\/]\)")) return false;
            if(Regex.IsMatch(formula, @"[^\+\-\*\/\(]\(")) return false;
            if(Regex.IsMatch(formula, @"\)[^\+\-\*\/\)]")) return false;

            formula = Regex.Replace(formula, @"\(|\)", "");
            formula = Regex.Replace(formula, @"[\+\-\*\/]", GlobalSeparator);
            var variables = SplitKeys(formula);
            foreach(var variable in variables) {
                if(Regex.IsMatch(formula, @"^\d+(\.\d+)?$")) continue;
                if(!Regex.IsMatch(formula, @"^@.+>>.+$")) return false;
                var starts = Regex.Matches(variable, @"@");
                if(starts.Count > 1) return false;
                var separators = Regex.Matches(variable, @">>");
                if(separators.Count > 1) return false;
            }

            return true;
        }

        public static List<string> GetFormulaVariables(string formula) {
            if(string.IsNullOrWhiteSpace(formula)) return null;
            formula = Regex.Replace(formula, @"\s+", "");
            formula = Regex.Replace(formula, @"\(|\)", "");
            formula = Regex.Replace(formula, @"[\+\-\*\/]", GlobalSeparator);
            var variables = SplitKeys(formula);
            var result = new List<string>();
            foreach(var variable in variables){
                if(Regex.IsMatch(formula, @"^\d+(\.\d+)?$")) continue;
                if(result.Contains(variable)) continue;
                result.Add(variable);
            }

            return result;
        }

        public static List<DateTime> GetDateSpan(DateTime start, DateTime end) {
            start = start.Date; end = end.Date;
            var dates = new List<DateTime>();
            while(start <= end) {
                dates.Add(start);
                start = start.AddDays(1);
            }
            return dates;
        }
    }
}