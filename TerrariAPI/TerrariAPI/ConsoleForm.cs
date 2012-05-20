using System;
using Microsoft.Xna.Framework;
using TerrariAPI.Commands;
using TerrariAPI.Hooking;
using XNAForms;
using XNAForms.Forms;

namespace TerrariAPI
{
    internal sealed class ConsoleForm : Form
    {
        private TextArea ta1;
        private TextBox tb1;

        internal ConsoleForm()
            : base(new Position(0, 0), new Size(400, 250), "Console")
        {
            ta1 = new TextArea(new Position(0, 0), new Size(0, 0));
            ta1.dockStyle = DockStyle.TOP;
            ta1.sizeFunction = () => new Size(size.width - 12, size.height - 12 - tb1.size.height);
            Add(ta1);

            tb1 = new TextBox(new Position(0, 0), 0);
            tb1.dockStyle = DockStyle.BOTTOM;
            tb1.onEnter += OnEnter;
            tb1.sizeFunction = () => new Size(size.width - 12, tb1.size.height);
            Add(tb1);

            onUpdate += () =>
            {
                if (rectangle.IntersectsMouse())
                {
                    Client.DisableMouse = true;
                }
                if (tb1.active)
                {
                    Client.DisableKeys = true;
                }
            };

            canResize = true;
            minSize = new Size(350, 200);
        }
        internal void AddMessage(string str, Color color)
        {
            ta1.Add(new Text(new Position(0, 0), color, str));
        }
        internal void Clear()
        {
            ta1.Clear();
        }
        private void OnEnter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.text != "")
            {
                AddMessage(">> " + tb.text, new Color(255, 255, 255));
                Command.Execute(tb.text);
                tb.Clear();
            }
        }
    }
}
