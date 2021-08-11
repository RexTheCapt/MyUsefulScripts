using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Linker
{
    public partial class FormMain : Form
    {
        readonly string LinksFile = "Links.json";

        public FormMain()
        {
            InitializeComponent();
            buttonAddLink.Enabled = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            listView1.GridLines = true;

            listView1.Columns.Add("From", 200);
            listView1.Columns.Add("To", 200);
            listView1.Columns.Add("Date", 200);

            listView1.ShowItemToolTips = true;

            listView1.DoubleClick += ListView1_DoubleClick;
            textBoxTo.TextChanged += TextBox_TextChanged;
            textBoxFrom.TextChanged += TextBox_TextChanged;

            ReadLinkedList();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxTo.Text) || string.IsNullOrWhiteSpace(textBoxTo.Text))
            {
                buttonAddLink.Enabled = false;
                textBoxTo.BackColor = Color.FromName("Window");
                textBoxFrom.BackColor = Color.FromName("Window");
                return;
            }

            if (string.IsNullOrEmpty(textBoxFrom.Text) || string.IsNullOrWhiteSpace(textBoxFrom.Text))
            {
                textBoxFrom.BackColor = Color.Red;
                buttonAddLink.Enabled = false;
                return;
            }
            else
            {
                textBoxFrom.BackColor = Color.FromName("Window");
            }

            if (Directory.Exists(textBoxTo.Text))
            {
                buttonAddLink.Enabled = true;
                textBoxTo.BackColor = Color.FromName("Window");
            }
            else
            {
                buttonAddLink.Enabled = false;
                textBoxTo.BackColor = Color.Red;
            }
        }

        private void ListView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                LinkInfo li = (LinkInfo)listView1.SelectedItems[0].Tag;
                FormEdit fe = new FormEdit(li);
                fe.ShowDialog();

                if (fe.DialogResult == DialogResult.OK)
                {
                    listView1.Items.Remove(listView1.SelectedItems[0]);
                    var v = listView1.Items.Add(new ListViewItem(fe.LinkInfo.ToArray()));
                    v.Tag = fe.LinkInfo;
                    v.BackColor = Color.Green;
                    SaveChanges();
                }
            }
        }

        private void ReadLinkedList()
        {
            if (File.Exists(LinksFile))
            {
                string rawText = "";

                using (StreamReader sr = new StreamReader(LinksFile))
                {

                    while (!sr.EndOfStream)
                    {
                        rawText += sr.ReadLine();
                    }
                }

                JObject jobject = (JObject)JsonConvert.DeserializeObject(rawText);
                JArray jArray = jobject.Value<JArray>("links");

                listView1.Items.Clear();

                foreach (JObject j in jArray)
                {
                    LinkInfo linkInfo = new LinkInfo(j);

                    ListViewItem lvi = new ListViewItem(linkInfo.ToArray());
                    lvi.Tag = linkInfo;

                    if (!JunctionPoint.Exists(linkInfo.From))
                        lvi.BackColor = Color.Red;
                    else if (JunctionPoint.GetTarget(linkInfo.From) != linkInfo.To || !Directory.Exists(linkInfo.To))
                        lvi.BackColor = Color.Yellow;

                    listView1.Items.Add(lvi);
                }
            }
        }

        private void ButtonAddLink_Click(object sender, EventArgs e)
        {
            LinkInfo li = new LinkInfo(textBoxFrom.Text, textBoxTo.Text);

            AddLink(li);

            textBoxTo.Text = "";
            textBoxFrom.Text = "";

            SaveChanges();
        }

        private void AddLink(LinkInfo link)
        {
            try
            {
                JunctionPoint.Create(link.From, link.To, false);
            }
            catch (Exception e)
            {
                Type t = e.GetType();

                if (t == typeof(IOException))
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                throw e;
            }

            var v = listView1.Items.Add(new ListViewItem(link.ToArray()));
            v.Tag = link;

            v.BackColor = Color.Green;
        }

        private void ButtonRemoveLink_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                LinkInfo link = (LinkInfo)item.Tag;
                JunctionPoint.Delete(link.From);

                listView1.Items.Remove(item);
            }

            SaveChanges();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            JObject jobject = new JObject();
            JArray jArray = new JArray();
            jobject.Add("links", jArray);

            foreach (ListViewItem lvi in listView1.Items)
            {
                LinkInfo link = (LinkInfo)lvi.Tag;
                JObject j = new JObject();
                j.Add("from", link.From);
                j.Add("to", link.To);
                j.Add("date", link.Date);

                jArray.Add(j);
            }

            using (StreamWriter sw = new StreamWriter(LinksFile))
                sw.WriteLine(JsonConvert.SerializeObject(jobject));
        }
    }
}
