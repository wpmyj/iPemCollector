﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iPem.Configurator {
    public partial class Main : Form {
        private Registry _registry;
        private TreeNode _currentNode;
        private TestStatus _testStatus = TestStatus.Default;

        public Main() {
            InitializeComponent();
            this.Text = iPemStore.Name;
        }

        private void Main_Load(object sender, EventArgs e) {
            try {
                _registry = new Registry(Application.StartupPath);
                _registry.CreateRegistry();

                this.BindCfgNodes(cfgNodeTree);

                planTypeField.ValueMember = "Id";
                planTypeField.DisplayMember = "Name";
                planTypeField.DataSource = Common.GetPlanStore();

                dbTypeField.ValueMember = "Id";
                dbTypeField.DisplayMember = "Name";
                dbTypeField.DataSource = Common.GetDbStore();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindCfgNodes(TreeView tv) {
            tv.Nodes.Clear();

            var root = new TreeNode { Name = "cfgRoot", Text = "配置管理项", ImageKey = "cfgRoot", SelectedImageKey = "cfgRoot", Tag = new TagModel { Type = NodeType.Root } };
            var paramNode = new TreeNode { Name = "cfgParamNode", Text = "常规", ImageKey = "cfgParam", SelectedImageKey = "cfgParam", Tag = new TagModel { Type = NodeType.Param } };
            var dbNode = new TreeNode { Name = "cfgDbNode", Text = "数据库", ImageKey = "cfgDb", SelectedImageKey = "cfgDb", Tag = new TagModel { Type = NodeType.Parent } };
            var planNode = new TreeNode { Name = "cfgPlanNode", Text = "计划任务", ImageKey = "cfgPlan", SelectedImageKey = "cfgPlan", Tag = new TagModel { Type = NodeType.Parent } };

            var databases = _registry.GetDatabases();
            if (databases != null && databases.Count > 0) {
                foreach (var db in databases) {
                    dbNode.Nodes.Add(new TreeNode { Name = db.Id, Text = db.Name, ImageKey = dbNode.ImageKey, SelectedImageKey = dbNode.SelectedImageKey, Tag = new TagModel { Type = NodeType.Database } });
                }
            }

            var plans = _registry.GetTasks();
            if (plans != null && plans.Count > 0) {
                foreach (var plan in plans.OrderBy(p=>p.Index)) {
                    planNode.Nodes.Add(new TreeNode { Name = plan.Id, Text = plan.Name, ImageKey = planNode.ImageKey, SelectedImageKey = planNode.SelectedImageKey, Tag = new TagModel { Type = NodeType.Plan } });
                }
            }

            tv.Nodes.Add(root);
            root.Nodes.AddRange(new TreeNode[] { paramNode, dbNode, planNode });
            tv.SelectedNode = root;
            tv.ExpandAll();
        }

        private void cfgNodeTree_AfterSelect(object sender, TreeViewEventArgs e) {
            try {
                _currentNode = e.Node;
                paramPanel.Dock = DockStyle.None;
                databasePanel.Dock = DockStyle.None;
                planPanel.Dock = DockStyle.None;
                paramPanel.Visible = false;
                databasePanel.Visible = false;
                planPanel.Visible = false;

                var tag = _currentNode.Tag as TagModel;
                if (tag.Type == NodeType.Param) {
                    paramPanel.Dock = DockStyle.Fill;
                    paramPanel.Visible = true;

                    //初始化
                    almIntervalField.Value = 1;
                    cfgIntervalField.Value = 1;
                    almSyncField.Value = 1;
                    initMaxCountField.Value = 1;

                    var curParam = _registry.GetParams().Find(p => p.Id == "P001");
                    if (curParam == null) throw new Exception("未找到参数配置信息");

                    if (!string.IsNullOrWhiteSpace(curParam.Json)) {
                        var paramModel = JsonConvert.DeserializeObject<ParamModel>(curParam.Json);
                        if (paramModel != null) {
                            almIntervalField.Value = paramModel.AlarmInterval;
                            cfgIntervalField.Value = paramModel.ConfigInterval;
                            almSyncField.Value = paramModel.AlarmSync;
                            initMaxCountField.Value = paramModel.InitRepeatCount;
                        } 
                    }

                    tag.Parameter = curParam;
                } else if (tag.Type == NodeType.Database) {
                    databasePanel.Dock = DockStyle.Fill;
                    databasePanel.Visible = true;

                    //初始化
                    dbTypeField.SelectedValue = (int)DatabaseType.SQLServer;
                    dbIPField.Text = "";
                    dbPortField.Value = 1433;
                    dbNameField.Text = "";
                    dbUidField.Text = "";
                    dbPwdField.Text = "";

                    var curDatabase = _registry.GetDatabases().Find(p => p.Id == _currentNode.Name);
                    if (curDatabase == null) throw new Exception("未找到参数配置信息");

                    dbTypeField.SelectedValue = (int)curDatabase.Type;
                    dbIPField.Text = curDatabase.IP ?? "";
                    dbPortField.Value = curDatabase.Port;
                    dbNameField.Text = curDatabase.Db ?? "";
                    dbUidField.Text = curDatabase.Uid ?? "";
                    dbPwdField.Text = curDatabase.Password ?? "";
                    tag.Parameter = curDatabase;
                } else if (tag.Type == NodeType.Plan) {
                    planPanel.Dock = DockStyle.Fill;
                    planPanel.Visible = true;

                    //初始化
                    planTypeField.SelectedValue = (int)PlanType.Hour;
                    planStartDateField.Value = DateTime.Parse("2017/01/01");
                    planEndDateField.Value = DateTime.Parse("2017/12/31");
                    planRateField.Value = 1;
                    planStartTimeField.Value = DateTime.Parse("2017/01/01 00:00:00");
                    planEndTimeField.Value = DateTime.Parse("2017/01/01 23:59:59");

                    var curTask = _registry.GetTasks().Find(p => p.Id == _currentNode.Name);
                    if (curTask == null) throw new Exception("未找到参数配置信息");
                    if (!string.IsNullOrWhiteSpace(curTask.Json)) {
                        var paramters = JsonConvert.DeserializeObject<TaskModel>(curTask.Json);
                        planTypeField.SelectedValue = (int)paramters.Type;
                        planStartDateField.Value = paramters.StartDate;
                        planEndDateField.Value = paramters.EndDate;
                        planRateField.Value = paramters.Interval;
                        planStartTimeField.Value = paramters.StartTime;
                        planEndTimeField.Value = paramters.EndTime;
                    }

                    tag.Parameter = curTask;
                }
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void planTypeField_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                var curValue = (int)planTypeField.SelectedValue;
                if (curValue == (int)PlanType.Hour) {
                    planRateComment.Text = "小时，执行一次。";
                    planTimeName.Text = "执行时段:";
                    planTimeToName.Visible = true;
                    planEndTimeField.Visible = true;
                } else if (curValue == (int)PlanType.Day) {
                    planRateComment.Text = "天，执行一次。";
                    planTimeName.Text = "执行时间:";
                    planTimeToName.Visible = false;
                    planEndTimeField.Visible = false;
                } else if (curValue == (int)PlanType.Month) {
                    planRateComment.Text = "月，执行一次。";
                    planTimeName.Text = "执行时间:";
                    planTimeToName.Visible = false;
                    planEndTimeField.Visible = false;
                }

                planCommentField.Text = this.GetPlanComment();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void topMenuItem0301_Click(object sender, EventArgs e) {
            Help.ShowHelp(this, String.Format(@"file://{0}\doc\configurator.chm", Environment.CurrentDirectory));
        }

        private void topMenuItem0302_Click(object sender, EventArgs e) {
            new About().ShowDialog();
        }

        private void paramSaveButton_Click(object sender, EventArgs e) {
            try {
                var tag = _currentNode.Tag as TagModel;
                if (tag.Type != NodeType.Param) throw new Exception("节点类型错误。");
                var paramter = (ParamEntity)tag.Parameter;
                paramter.Json = JsonConvert.SerializeObject(new ParamModel { AlarmInterval = (int)almIntervalField.Value, ConfigInterval = (int)cfgIntervalField.Value, AlarmSync = (int)almSyncField.Value, InitRepeatCount = (int)initMaxCountField.Value });
                paramter.Time = DateTime.Now;
                _registry.SaveParams(new List<ParamEntity> { paramter });
                MessageBox.Show("保存成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dbTestButton_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrWhiteSpace(dbIPField.Text)) {
                    dbIPField.Focus();
                    MessageBox.Show("数据库地址不能为空", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(dbNameField.Text)) {
                    dbNameField.Focus();
                    MessageBox.Show("数据库名称不能为空", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(dbUidField.Text)) {
                    dbUidField.Focus();
                    MessageBox.Show("登录名不能为空", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(dbPwdField.Text)) {
                    dbPwdField.Focus();
                    MessageBox.Show("密码不能为空", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cfgNodeTree.Enabled = false;
                dbTypeField.Enabled = false;
                dbIPField.Enabled = false;
                dbPortField.Enabled = false;
                dbNameField.Enabled = false;
                dbUidField.Enabled = false;
                dbPwdField.Enabled = false;
                dbTestButton.Enabled = false;
                dbSaveButton.Enabled = false;
                var connectionString = SqlTypeConverter.CreateConnectionString(false, dbIPField.Text.Trim(), (int)dbPortField.Value, dbNameField.Text.Trim(), dbUidField.Text.Trim(), dbPwdField.Text, 120);
                var testthread = new Thread(() => {
                    try {
                        var message = "";
                        var timeout = 30;
                        var thread = new Thread(() => {
                            using (var conn = new SqlConnection(connectionString)) {
                                try {
                                    conn.Open();
                                    conn.Close();
                                    _testStatus = TestStatus.Success;
                                } catch (Exception err) {
                                    _testStatus = TestStatus.Failure;
                                    message = err.Message;
                                }
                            }
                        });
                        _testStatus = TestStatus.Testing;
                        thread.IsBackground = true;
                        thread.Start();

                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        var ts = TimeSpan.FromSeconds(timeout);
                        while (sw.Elapsed < ts) {
                            thread.Join(TimeSpan.FromMilliseconds(500));
                            if (_testStatus != TestStatus.Testing) { break; }
                        }
                        sw.Stop();

                        if (_testStatus == TestStatus.Testing) {
                            MessageBox.Show("SQL Server服务器连接超时", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        } else if (_testStatus == TestStatus.Success) {
                            MessageBox.Show("测试连接成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        } else if (_testStatus == TestStatus.Failure) {
                            MessageBox.Show(message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    } catch (Exception err) {
                        MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } finally {
                        this.Invoke(new MethodInvoker(delegate {
                            cfgNodeTree.Enabled = true;
                            dbTypeField.Enabled = true;
                            dbIPField.Enabled = true;
                            dbPortField.Enabled = true;
                            dbNameField.Enabled = true;
                            dbUidField.Enabled = true;
                            dbPwdField.Enabled = true;
                            dbTestButton.Enabled = true;
                            dbSaveButton.Enabled = true;
                            dbCloseButton.Enabled = true;
                        }));
                        _testStatus = TestStatus.Default;
                    }
                });
                testthread.IsBackground = true;
                testthread.Start();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dbSaveButton_Click(object sender, EventArgs e) {
            try {
                var tag = _currentNode.Tag as TagModel;
                if (tag.Type != NodeType.Database) throw new Exception("节点类型错误。");

                var v = Convert.ToInt32(dbTypeField.SelectedValue);
                var database = (DbEntity)tag.Parameter;
                database.Type = Enum.IsDefined(typeof(DatabaseType),v) ? (DatabaseType)v : DatabaseType.SQLServer;
                database.IP = dbIPField.Text.Trim();
                database.Port = (int)dbPortField.Value;
                database.Db = dbNameField.Text.Trim();
                database.Uid = dbUidField.Text.Trim();
                database.Password = dbPwdField.Text;

                _registry.SaveDatabases(new List<DbEntity> { database });
                MessageBox.Show("保存成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dbCloseButton_Click(object sender, EventArgs e) {
            try {
                if (_testStatus != TestStatus.Default) {
                    dbCloseButton.Enabled = false;
                    _testStatus = TestStatus.Stop;
                } else {
                    this.Quit();
                }
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void planField_ValueChanged(object sender, EventArgs e) {
            try {
                planCommentField.Text = this.GetPlanComment();
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void planSaveButton_Click(object sender, EventArgs e) {
            try {
                var tag = _currentNode.Tag as TagModel;
                if (tag.Type != NodeType.Plan) throw new Exception("节点类型错误。");

                if (planStartDateField.Value > planEndDateField.Value) 
                    throw new Exception("开始日期必须小于或等于结束日期。");

                if (planStartTimeField.Value > planEndTimeField.Value)
                    throw new Exception("开始时间必须小于或等于结束时间。");

                var v = Convert.ToInt32(planTypeField.SelectedValue);
                var task = (TaskEntity)tag.Parameter;
                var model = new TaskModel {
                    Type = Enum.IsDefined(typeof(PlanType), v) ? (PlanType)v : PlanType.Hour,
                    StartDate = planStartDateField.Value,
                    EndDate = planEndDateField.Value,
                    Interval = (int)planRateField.Value,
                    StartTime = planStartTimeField.Value,
                    EndTime = planEndTimeField.Value
                };

                task.Json = JsonConvert.SerializeObject(model);
                _registry.SaveTasks(new List<TaskEntity> { task });
                MessageBox.Show("保存成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception err) {
                MessageBox.Show(err.Message, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Quit_Click(object sender, EventArgs e) {
            this.Quit();
        }

        private string GetPlanComment() {
            var curValue = (int)planTypeField.SelectedValue;
            if (curValue == (int)PlanType.Hour) {
                return string.Format("计划在{0}至{1}有效期内，每天在{2}至{3}之间，每{4}小时执行一次。", Common.ToDateString(planStartDateField.Value), Common.ToDateString(planEndDateField.Value), Common.ToTimeString(planStartTimeField.Value), Common.ToTimeString(planEndTimeField.Value), planRateField.Value == 1 ? "" : planRateField.Value.ToString());
            } else if (curValue == (int)PlanType.Day) {
                return string.Format("计划在{0}至{1}有效期内，每{2}天的{3}执行一次。", Common.ToDateString(planStartDateField.Value), Common.ToDateString(planEndDateField.Value), planRateField.Value == 1 ? "" : planRateField.Value.ToString(), Common.ToTimeString(planStartTimeField.Value));
            } else if (curValue == (int)PlanType.Month) {
                return string.Format("计划在{0}至{1}有效期内，每{2}月执行一次，在当月第1天的{3}执行。", Common.ToDateString(planStartDateField.Value), Common.ToDateString(planEndDateField.Value), planRateField.Value == 1 ? "" : planRateField.Value.ToString(), Common.ToTimeString(planStartTimeField.Value));
            }

            return string.Empty;
        }

        private void Quit() {
            if (MessageBox.Show("您确定要退出吗？", "确认对话框", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK) {
                Application.Exit();
            }
        }
    }
}
