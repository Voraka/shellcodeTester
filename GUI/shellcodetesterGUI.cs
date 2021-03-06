﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace shellcodeTester
{
    public partial class shellcodeTesterGUI : Form
    {
        public shellcodeTesterGUI()
        {
            InitializeComponent();
            disasmSc_RTB.AppendText("Disasembled shellcode will appear here\n");            
        }

        byte[] shellcode = new byte[0];
        uint architecture = 0;
        
        private void disasmBT_Click(object sender, EventArgs e)
        {
            bool addressOffsets = false;
            disasmSc_RTB.Clear();
            string insertedShellcode = insertScRTB.Text;
            insertedShellcode = insertedShellcode.Replace("\\x", string.Empty);
            insertedShellcode = insertedShellcode.Replace("0x", string.Empty);
            insertedShellcode = insertedShellcode.Replace(", ", string.Empty);
            insertedShellcode = insertedShellcode.Replace("\n", string.Empty);
            insertedShellcode = System.Text.RegularExpressions.Regex.Replace(insertedShellcode, @"\W+", "");
            shellcode = new byte[insertedShellcode.Length];
            try
            {
                for (int i = 0; i < insertedShellcode.Length; i += 2)
                    shellcode[i / 2] = Convert.ToByte(insertedShellcode.Substring(i, 2), 16);

                int lastIndex = Array.FindLastIndex(shellcode, b => b != 0);
                Array.Resize(ref shellcode, lastIndex + 1);

                if (architecture != 0)
                {
                    if (showAddresses.Checked)
                        addressOffsets = true;
                    disassemble disasm = new disassemble();
                    disasm.disassembleSC(shellcode, architecture, disasmSc_RTB, addressOffsets);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Please select an architecutre and insert shellcode into the listbox");
                }
            }
            catch
            {
                MessageBox.Show("Invalid shellcode detected. Only use shellcode in the form of \n\"\\x##\" \n\"0x##\" \n##");
            }         

        }

        private void x86_CheckedChanged(object sender, EventArgs e)
        {
            architecture = 32;
        }

        private void x64_CheckedChanged(object sender, EventArgs e)
        {
            architecture = 64;
        }

        private void fireSc_Click(object sender, EventArgs e)
        {
            if (shellcode.Length > 0)
            {
                shellcodeTester.fireShellcode(architecture, shellcode, targetCalc_CB.Checked);
            }
            else
                System.Windows.Forms.MessageBox.Show("Please put shellcode into the text field and disassemble before launching shellcode");
        }
       
    }
}
