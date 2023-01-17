using CommonTool;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFrame
{
    public partial class Form_Hive : Form
    {
        public Form_Hive()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            textBox2.Text = null;
            comboBox1.SelectedIndex = -1;
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            richTextBox_Hive.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = null;
            string responseString = null;
            if (checkBox1.Checked)
            {
                HiveMgr.LastError.tm_begin = !checkBox2.Checked ? dateTimePicker2.Value : DateTime.Now;

                HiveMgr.LastError.tm_end = checkBox2.Checked ? DateTime.Now : dateTimePicker2.Value;
                HiveMgr.LastError.Level = checkBox2.Checked ? "error" : comboBox1.Text;
                HiveMgr.LastError.strMsgEnglish = checkBox2.Checked ? "E-Stop pressed" : textBox1.Text;
                HiveMgr.LastError.code = checkBox2.Checked ? "F01SCOO-01-10" : textBox2.Text;

                HiveMgr.GetInstance().UploadInformation(HiveMgr.LastError, out message, false);

                Thread.Sleep(100);
                bool rf = HiveMgr.GetInstance().post(SystemMgr.GetInstance().GetParamString($"{DataStyle.ErrorData}Url"), message, ref responseString);

                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, rf, responseString);
                HiveMgr.LastError = new ErrorMessage();
            }
            else
            {
                HiveMgr.LastError.tm_begin = !checkBox2.Checked ? dateTimePicker2.Value : DateTime.Now;
                StateMessage LS = GetErrorInfo.GetInstance().ReadMachineStateXML();
                string OldState = ((MachineState)LS.MachineState).ToString();
                HiveMgr.ChangeState(new StateMessage()
                {
                    MachineState = (int)MachineState.UnplannedDown,
                    code = !checkBox2.Checked ? textBox2.Text : "F01SCOO-01-10",
                    strMsg = !checkBox2.Checked ? textBox1.Text : "E-Stop pressed",
                    strLevel = !checkBox2.Checked ? comboBox1.Text : "error"
                },
                    out message);
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, OldState, "UnplannedDown", true);
                Thread.Sleep(1000);

                HiveMgr.LastError.tm_end = checkBox2.Checked ? DateTime.Now : dateTimePicker2.Value;
                HiveMgr.LastError.Level = checkBox2.Checked ? "error" : comboBox1.Text;
                HiveMgr.LastError.strMsgEnglish = checkBox2.Checked ? "E-Stop pressed" : textBox1.Text;
                HiveMgr.LastError.code = checkBox2.Checked ? "F01SCOO-01-10" : textBox2.Text;

                HiveMgr.GetInstance().UploadInformation(HiveMgr.LastError, out message);
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message);
                HiveMgr.LastError = new ErrorMessage();

                Thread.Sleep(1000);
                HiveMgr.ChangeState(new StateMessage() { MachineState = (int)MachineState.Idle }, out message);
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, "UnplannedDown", "Idle", true);
            }

        }

        private void Form_Hive_Load(object sender, EventArgs e)
        {
            foreach (MachineState MS in Enum.GetValues(typeof(MachineState)))
            {
                comboBox2.Items.Add(MS);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 4)
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message;
            string responseString = null;
            StateMessage LS = GetErrorInfo.GetInstance().ReadMachineStateXML();
            string OldState = ((MachineState)LS.MachineState).ToString();

            if (checkBox4.Checked)
            {
                HiveMgr.ChangeState(new StateMessage()
                {
                    MachineState = checkBox3.Checked ? 5 : (int)(comboBox2.SelectedIndex + 1),
                    code = "F01SCOO-01-10",
                    strMsg = "E-Stop pressed",
                    strLevel = "critical"
                },
                    out message, false, null, LS.MachineState, "manual test", checkBox3.Checked ? "V2.1" : textBox_SW_version.Text.Trim());

                bool rf = HiveMgr.GetInstance().post(SystemMgr.GetInstance().GetParamString($"{DataStyle.MachineState}Url"), message, ref responseString);

                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, rf, responseString, OldState, ((MachineState)GetErrorInfo.GetInstance().ReadMachineStateXML().MachineState).ToString());
            }
            else
            {
                HiveMgr.ChangeState(new StateMessage()
                {
                    MachineState = checkBox3.Checked ? 5 : (int)(comboBox2.SelectedIndex + 1),
                    code = "F01SCOO-01-10",
                    strMsg = "E-Stop pressed",
                    strLevel = "critical"
                },
                    out message, true, null, LS.MachineState, "manual test", checkBox3.Checked ? "V2.1" : textBox_SW_version.Text.Trim());
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, OldState, ((MachineState)GetErrorInfo.GetInstance().ReadMachineStateXML().MachineState).ToString(), true);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string responseString = null;

            if (checkBox5.Checked)
            {
                string Json_Message;
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("measure_version", "v10.0.1");


                if (checkBox6.Checked)
                {
                    HiveMgr.GetInstance().EnvironmentalDataUpload(out Json_Message, 1, DateTime.Now, "34.7", "C", null, false);

                    bool rf = HiveMgr.GetInstance().post(SystemMgr.GetInstance().GetParamString($"{DataStyle.EnvironmentData}Url"), Json_Message, ref responseString);
                    HiveMgr.GetInstance().showLog(richTextBox_Hive, Json_Message, rf, responseString);

                }
                else
                {
                    HiveMgr.GetInstance().EnvironmentalDataUpload(out Json_Message, 1, DateTime.Now, "34.7", "C", null);
                    HiveMgr.GetInstance().showLog(richTextBox_Hive, Json_Message);
                }

            }
            else
            {
                string Value = textBox_value.Text.Trim();
                string Unit = textBox_unit.Text.Trim();
                int Sequence = (int)numericUpDown1.Value;
                string Json_Message;
                Dictionary<string, object> data = new Dictionary<string, object>();

                for (int i = 0; i < dataGridView_Envir.Rows.Count - 1; i++)
                {
                    data.Add(dataGridView_Envir.Rows[i].Cells[0].Value.ToString(), dataGridView_Envir.Rows[i].Cells[1].Value.ToString());
                }

                if (checkBox6.Checked)
                {
                    HiveMgr.GetInstance().EnvironmentalDataUpload(out Json_Message, Sequence, DateTime.Now, Value, Unit, data.Count > 0 ? data : null, false);

                    bool rf = HiveMgr.GetInstance().post(SystemMgr.GetInstance().GetParamString($"{DataStyle.ErrorData}Url"), Json_Message, ref responseString);
                    HiveMgr.GetInstance().showLog(richTextBox_Hive, Json_Message, rf, responseString);
                }
                else
                {
                    HiveMgr.GetInstance().EnvironmentalDataUpload(out Json_Message, Sequence, DateTime.Now, Value, Unit, data.Count > 0 ? data : null);
                    HiveMgr.GetInstance().showLog(richTextBox_Hive, Json_Message);
                }

            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox_value.Text = null;
            textBox_unit.Text = null;
            numericUpDown1.Value = 1;
            dataGridView_Envir.Rows.Clear();
        }

        private void rbtnMachineData1_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnMachineData1.Checked)
            {
                return;
            }
            rtxtJsonFormat.Clear();
            string Json_Message;
            rbtnMachineData1.Checked = true;
            rbtnMachineData2.Checked = false;
            rbtnMachineData3.Checked = false;

            Dictionary<string, object> Serialtemp = new Dictionary<string, object>();
            Serialtemp.Add("carrier_sn", "0010");
            Serialtemp.Add("barcode", "STC1234567890");
            Serialtemp.Add("wip_serials", new List<string>() { "1111111111", "22222222222222", "333333333333" });
            List<HiveMgr.MachineData.Activity> t = new List<HiveMgr.MachineData.Activity>();

            HiveMgr.MachineData.Activity aty = new HiveMgr.MachineData.Activity();
            aty.end_time = DateTime.Now.ToString(HiveMgr.TimeFormat);
            aty.start_time = DateTime.Now.ToString(HiveMgr.TimeFormat);
            aty.stage = "right";
            aty.activity = "robot Pick";
            t.Add(aty);

            HiveMgr.MachineData.Activity aty1 = new HiveMgr.MachineData.Activity();
            aty1.end_time = DateTime.Now.ToString(HiveMgr.TimeFormat);
            aty1.start_time = DateTime.Now.ToString(HiveMgr.TimeFormat);
            aty1.stage = "left";
            aty1.activity = "robot Place";
            t.Add(aty1);

            DateTime begintime = DateTime.Now;
            Thread.Sleep(100);

            HiveMgr.GetInstance().MachineDataUpload(out Json_Message, "true", begintime, DateTime.Now, Serialtemp, t, null, true);

            rtxtJsonFormat.AppendText(HiveMgr.GetInstance().ConvertJsonString(Json_Message));

        }

        private void rbtnMachineData2_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnMachineData2.Checked)
            {
                return;
            }
            rtxtJsonFormat.Clear();
            rbtnMachineData1.Checked = false;
            rbtnMachineData2.Checked = true;
            rbtnMachineData3.Checked = false;
            string Json_Message;

            Dictionary<string, object> Serialtemp = new Dictionary<string, object>();
            Serialtemp.Add("carrier_sn", "0010");
            Serialtemp.Add("barcode", "STC1234567890");
            Serialtemp.Add("NCcode", "NC1234567890");
            Serialtemp.Add("Coilcode", "Coil1234567890");
            Serialtemp.Add("wip_serials", new List<string>() { "SN1111111111", "SN22222222222", "SN33333333333" });


            Dictionary<string, object> t1 = new Dictionary<string, object>();
            t1.Add("MeasurementA", "0.56");
            t1.Add("MeasurementB", "0.55");
            t1.Add("MeasurementC", "0.5566");

            //Tossing
            Dictionary<string, string>[] Tossing = new Dictionary<string, string>[] { new Dictionary<string, string>() };
            Tossing[0].Add("Coil", "0");
            t1.Add("Tossing", Tossing[0]);
            t1.Add("Tossing_reason", "Sacn fail");


            DateTime begintime = DateTime.Now;
            Thread.Sleep(100);

            HiveMgr.GetInstance().MachineDataUpload(out Json_Message, "true", begintime, DateTime.Now, Serialtemp, t1, null, false, null, true);

            rtxtJsonFormat.AppendText(HiveMgr.GetInstance().ConvertJsonString(Json_Message));
        }

        private void rbtnMachineData3_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnMachineData3.Checked)
            {
                return;
            }
            rtxtJsonFormat.Clear();
            rbtnMachineData1.Checked = false;
            rbtnMachineData2.Checked = false;
            rbtnMachineData3.Checked = true;
            string Json_Message;


            Dictionary<string, object> Serialtemp = new Dictionary<string, object>();
            Serialtemp.Add("carrier_sn", "0010");
            Serialtemp.Add("barcode", "STC1234567890");
            Serialtemp.Add("NCcode", "NC1234567890");
            Serialtemp.Add("Coilcode", "Coil1234567890");
            Serialtemp.Add("wip_serials", new List<string>() { "SN1111111111", "SN22222222222", "SN33333333333" });


            Dictionary<string, object> t1 = new Dictionary<string, object>();
            t1.Add("MeasurementA", "0.56");
            t1.Add("MeasurementB", "0.55");
            t1.Add("MeasurementC", "0.5566");
            t1.Add("sw_version", "HIVE_TEST_1.0");
            t1.Add("limits_version", "HIVE_LIMIT_1.0");

            //Tossing
            Dictionary<string, string>[] Tossing = new Dictionary<string, string>[] { new Dictionary<string, string>() };
            Tossing[0].Add("Coil", "0");
            t1.Add("Tossing", Tossing[0]);
            t1.Add("Tossing_reason", "Sacn fail");

            //一级目录
            Dictionary<string, object> limit = new Dictionary<string, object>();

            //二级目录
            Dictionary<string, string>[] limitchild = new Dictionary<string, string>[] { new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>() };

            limitchild[0].Add("upper_limit", "1.0");
            limitchild[0].Add("lower_limit", "0.1");

            limitchild[1].Add("upper_limit", "1.0");

            limitchild[2].Add("lower_limit", "0");


            limit.Add("MeasurementA", limitchild[0]);
            limit.Add("MeasurementB", limitchild[1]);
            limit.Add("MeasurementC", limitchild[2]);

            DateTime begintime = DateTime.Now;
            Thread.Sleep(100);

            HiveMgr.GetInstance().MachineDataUpload(out Json_Message, "true", begintime, DateTime.Now, Serialtemp, t1, null, true, limit, true);

            rtxtJsonFormat.AppendText(HiveMgr.GetInstance().ConvertJsonString(Json_Message));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            rtxtJsonFormat.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string message;
            string responseString = null;

            rtxtJsonFormat.SelectAll();
            message = rtxtJsonFormat.SelectedText;

            if (message == null || message == "")
            {
                richTextBox_Hive.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" + "  " + "Please select data type!\r\n");
                MessageBox.Show("上传数据为空，请选择需要上传的数据类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            message = HiveMgr.GetInstance().Compress(message);

            if (checkBox8.Checked)
            {

                bool rf = HiveMgr.GetInstance().post(SystemMgr.GetInstance().GetParamString($"{DataStyle.MachineData}Url"), message, ref responseString);
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message, rf, responseString);
            }
            else
            {
                UpInformation(message);
                HiveMgr.GetInstance().showLog(richTextBox_Hive, message);
            }


        }



        public void UpInformation(string jsonss)
        {

            string hiveDirectory = SystemMgr.GetInstance().GetParamString("HiveDir");
            string directory = SystemMgr.GetInstance().GetParamString("HiveDir");

            if (hiveDirectory.LastIndexOf("\\") != directory.Length - 1)
                hiveDirectory += "\\";

            if (PostMgr.PostPause)
            {
                directory = hiveDirectory + "UploadFiles\\machinedata\\";
            }
            else
            {
                directory = hiveDirectory + "UploadFilesPause\\machinedata\\";
            }


            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);


            string fileName = directory + System.Guid.NewGuid().ToString() + ".json";

            File.AppendAllText(fileName, jsonss);
        }



        private void button11_Click(object sender, EventArgs e)
        {
            if (button11.BackColor != Color.Green)
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.MachineData - 1] = true;
                button11.BackColor = Color.Green;
            }
            else
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.MachineData - 1] = false;
                button11.BackColor = Color.Gray;
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (button12.BackColor != Color.Green)
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.ErrorData - 1] = true;
                button12.BackColor = Color.Green;
            }
            else
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.ErrorData - 1] = false;
                button12.BackColor = Color.Gray;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (button13.BackColor != Color.Green)
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.MachineState - 1] = true;
                button13.BackColor = Color.Green;
            }
            else
            {
                HiveMgr.BackUp_SendFlag[(int)DataStyle.MachineState - 1] = false;
                button13.BackColor = Color.Gray;
            }
        }

        private void Form_Hive_FormClosing(object sender, FormClosingEventArgs e)
        {            
            GetErrorInfo.GetInstance().WriteMachineStateXML(new StateMessage { }, true);
        }
    }

}
