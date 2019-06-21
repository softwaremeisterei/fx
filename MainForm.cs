using System;
using System.ComponentModel;
using System.Windows.Forms;

public class MainForm : Form
{
    private Timer timer1;
    private PhysicsView physicsView1;
    private IContainer components;

    public MainForm()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.physicsView1 = new PhysicsView();
        this.SuspendLayout();
        // 
        // physicsView1
        // 
        this.physicsView1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.physicsView1.Name = "physicsView1";
        this.physicsView1.Size = new System.Drawing.Size(632, 477);
        this.physicsView1.TabIndex = 0;
        // 
        // Form1
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(632, 477);
        this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.physicsView1});
        this.Name = "Form1";
        this.Text = "Form1";
        this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
        this.Load += new System.EventHandler(this.Form1_Load);
        this.ResumeLayout(false);

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.Run(new MainForm());
    }

    private void Form1_Load(object sender, System.EventArgs e)
    {
        this.physicsView1.Focus();
    }

    private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        this.physicsView1.Shutdown();
    }
}
