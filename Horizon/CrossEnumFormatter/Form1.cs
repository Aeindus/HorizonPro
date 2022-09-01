using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossEnumFormatter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            string str = textBox1.Text;
            int starting_index = 0;

            Regex rx = new Regex(@"=\s*\d+");
            MatchEvaluator evaluator = new MatchEvaluator(match =>{ 
                starting_index++;  
                return " = "+starting_index.ToString(); 
            });

            
            textBox2.Text = rx.Replace(str, evaluator);
        }
    }
}
