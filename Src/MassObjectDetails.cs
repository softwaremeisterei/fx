using System;
using System.ComponentModel;
using System.Windows.Forms;

public class MassObjectDetails : Form
{
    public MassObject MassObject = new MassObject();

    private Button btnSave;
    private TextBox tbPosx;
    private TextBox tbPosy;
    private TextBox tbMass;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private TextBox tbOrx;
    private TextBox tbOry;
    private Label label8;
    private TextBox tbSpeed;
    private Label label9;
    private TextBox tbDiameter;

    private Container components = null;

    public MassObjectDetails()
    {
        InitializeComponent();
    }

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
        this.btnSave = new System.Windows.Forms.Button();
        this.tbPosx = new System.Windows.Forms.TextBox();
        this.tbOrx = new System.Windows.Forms.TextBox();
        this.tbPosy = new System.Windows.Forms.TextBox();
        this.tbMass = new System.Windows.Forms.TextBox();
        this.tbOry = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        this.label8 = new System.Windows.Forms.Label();
        this.tbSpeed = new System.Windows.Forms.TextBox();
        this.label9 = new System.Windows.Forms.Label();
        this.tbDiameter = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // btnSave
        // 
        this.btnSave.Location = new System.Drawing.Point(120, 192);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(56, 23);
        this.btnSave.TabIndex = 7;
        this.btnSave.Text = "Save";
        this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        // 
        // tbPosx
        // 
        this.tbPosx.Location = new System.Drawing.Point(120, 152);
        this.tbPosx.Name = "tbPosx";
        this.tbPosx.Size = new System.Drawing.Size(56, 20);
        this.tbPosx.TabIndex = 5;
        this.tbPosx.Text = "";
        // 
        // tbOrx
        // 
        this.tbOrx.Location = new System.Drawing.Point(120, 120);
        this.tbOrx.Name = "tbOrx";
        this.tbOrx.Size = new System.Drawing.Size(56, 20);
        this.tbOrx.TabIndex = 3;
        this.tbOrx.Text = "";
        // 
        // tbPosy
        // 
        this.tbPosy.Location = new System.Drawing.Point(216, 152);
        this.tbPosy.Name = "tbPosy";
        this.tbPosy.Size = new System.Drawing.Size(56, 20);
        this.tbPosy.TabIndex = 6;
        this.tbPosy.Text = "";
        // 
        // tbMass
        // 
        this.tbMass.Location = new System.Drawing.Point(120, 24);
        this.tbMass.Name = "tbMass";
        this.tbMass.Size = new System.Drawing.Size(56, 20);
        this.tbMass.TabIndex = 1;
        this.tbMass.Text = "";
        // 
        // tbOry
        // 
        this.tbOry.Location = new System.Drawing.Point(216, 120);
        this.tbOry.Name = "tbOry";
        this.tbOry.Size = new System.Drawing.Size(56, 20);
        this.tbOry.TabIndex = 4;
        this.tbOry.Text = "";
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(16, 152);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(48, 23);
        this.label1.TabIndex = 2;
        this.label1.Text = "Position";
        // 
        // label2
        // 
        this.label2.Location = new System.Drawing.Point(16, 120);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(64, 23);
        this.label2.TabIndex = 2;
        this.label2.Text = "Orientation";
        // 
        // label3
        // 
        this.label3.Location = new System.Drawing.Point(16, 24);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(40, 23);
        this.label3.TabIndex = 2;
        this.label3.Text = "Mass";
        // 
        // label4
        // 
        this.label4.Location = new System.Drawing.Point(96, 152);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(16, 23);
        this.label4.TabIndex = 2;
        this.label4.Text = "x";
        // 
        // label5
        // 
        this.label5.Location = new System.Drawing.Point(96, 120);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(16, 23);
        this.label5.TabIndex = 2;
        this.label5.Text = "x";
        // 
        // label6
        // 
        this.label6.Location = new System.Drawing.Point(192, 120);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(16, 23);
        this.label6.TabIndex = 2;
        this.label6.Text = "y";
        // 
        // label7
        // 
        this.label7.Location = new System.Drawing.Point(192, 152);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(16, 23);
        this.label7.TabIndex = 2;
        this.label7.Text = "y";
        // 
        // label8
        // 
        this.label8.Location = new System.Drawing.Point(16, 88);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(40, 23);
        this.label8.TabIndex = 2;
        this.label8.Text = "Speed";
        // 
        // tbSpeed
        // 
        this.tbSpeed.Location = new System.Drawing.Point(120, 88);
        this.tbSpeed.Name = "tbSpeed";
        this.tbSpeed.Size = new System.Drawing.Size(56, 20);
        this.tbSpeed.TabIndex = 2;
        this.tbSpeed.Text = "";
        // 
        // label9
        // 
        this.label9.Location = new System.Drawing.Point(16, 56);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(64, 23);
        this.label9.TabIndex = 2;
        this.label9.Text = "Diameter";
        // 
        // tbDiameter
        // 
        this.tbDiameter.Location = new System.Drawing.Point(120, 56);
        this.tbDiameter.Name = "tbDiameter";
        this.tbDiameter.Size = new System.Drawing.Size(56, 20);
        this.tbDiameter.TabIndex = 1;
        this.tbDiameter.Text = "";
        // 
        // PhysicObjectForm
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(480, 325);
        this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.label1,
                                                                          this.tbPosx,
                                                                          this.btnSave,
                                                                          this.tbOrx,
                                                                          this.tbPosy,
                                                                          this.tbMass,
                                                                          this.tbOry,
                                                                          this.label2,
                                                                          this.label3,
                                                                          this.label4,
                                                                          this.label5,
                                                                          this.label6,
                                                                          this.label7,
                                                                          this.label8,
                                                                          this.tbSpeed,
                                                                          this.label9,
                                                                          this.tbDiameter});
        this.Name = "PhysicObjectForm";
        this.Text = "PhysicObjectForm";
        this.Load += new System.EventHandler(this.PhysicObjectForm_Load);
        this.ResumeLayout(false);

    }
    #endregion

    private void btnSave_Click(object sender, System.EventArgs e)
    {
        this.DialogResult = DialogResult.OK;
        System.Globalization.NumberStyles ns =
            System.Globalization.NumberStyles.Float;
        this.MassObject.Position.X = Double.Parse(this.tbPosx.Text, ns);
        this.MassObject.Position.Y = Double.Parse(this.tbPosy.Text, ns);
        this.MassObject.Speed = Double.Parse(this.tbSpeed.Text);
        this.MassObject.Direction.X = Double.Parse(this.tbOrx.Text, ns);
        this.MassObject.Direction.Y = Double.Parse(this.tbOry.Text, ns);
        this.MassObject.Mass = Double.Parse(this.tbMass.Text, ns);
        this.Close();
    }

    private void PhysicObjectForm_Load(object sender, System.EventArgs e)
    {
        this.tbPosx.Text = this.MassObject.Position.X.ToString();
        this.tbPosy.Text = this.MassObject.Position.Y.ToString();
        this.tbOrx.Text = this.MassObject.Direction.X.ToString();
        this.tbOry.Text = this.MassObject.Direction.Y.ToString();
        this.tbMass.Text = this.MassObject.Mass.ToString();
        this.tbSpeed.Text = this.MassObject.Speed.ToString();

        this.tbMass.Select();
        this.tbMass.SelectAll();
        this.AcceptButton = this.btnSave;
    }
}
