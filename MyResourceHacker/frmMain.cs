using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Win32;
namespace MyResourceHacker
{
    public partial class frmMain : Form
    {
        string fileName = string.Empty;
        Assembly assem;
        Type selectedType=typeof(Int32);
        object selectedTypeInstance=null;
        public frmMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void selectDllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = ".Net Dlls|*.dll";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = dialog.FileName;
                PopulateListBoxes();
            }
            
        }

        private void PopulateListBoxes()
        {
            Reset();
            assem = Assembly.LoadFile(fileName);
            if (assem.GetTypes().Count() > 0)
            {
                btnGetInfo.Enabled = true;
            }
            else
            {
                btnGetInfo.Enabled = false ;
            }

            foreach (Type oType in assem.GetTypes())
            {
                cmbType.Items.Add(oType);
                MemberInfo[] members = oType.GetMembers();
                foreach (MemberInfo member in members)
                {
                    switch (member.MemberType)
                    {

                        case MemberTypes.Constructor:
                            lstConstructor.Items.Add(member.Name);
                            break;

                        case MemberTypes.Event:
                            lstEvents.Items.Add(member.Name);
                            break;

                        case MemberTypes.Method:
                            lstMethods.Items.Add(member.Name);
                            break;

                        case MemberTypes.Property:
                            lstProperties.Items.Add(member.Name);
                            break;

                        default:
                            lstOthers.Items.Add(member.Name);
                            break;
                    }


                }

            }
            cmbType.SelectedIndex = 0;
        }

      


        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstConstructor.Items.Clear ();
            lstEvents.Items.Clear ();
            lstMethods.Items.Clear ();
            lstProperties.Items.Clear ();
            Type selectedType = assem.GetType ( cmbType.Text );
            MemberInfo[] members = selectedType.GetMembers ();
            foreach ( MemberInfo member in members )
            {
                switch ( member.MemberType )
                {

                    case MemberTypes.Constructor:
                        lstConstructor.Items.Add ( member.Name );
                        break;

                    case MemberTypes.Event:
                        lstEvents.Items.Add ( member.Name );
                        break;

                    case MemberTypes.Method:
                        lstMethods.Items.Add ( member.Name );
                        break;

                    case MemberTypes.Property:
                        lstProperties.Items.Add ( member.Name );
                        break;

                    default:
                        lstOthers.Items.Add ( member.Name );
                        break;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Type selectedType = assem.GetType(cmbType.Text);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("AssemblyQualifiedName: {0}", selectedType.AssemblyQualifiedName));
            sb.AppendLine(string.Format("Namespace: {0}", selectedType.Namespace));
            sb.AppendLine(string.Format("Is IsClass: {0}", selectedType.IsClass));
            sb.AppendLine(string.Format("BaseType: {0}", selectedType.BaseType));
            sb.AppendLine(string.Format("Is ValueType: {0}", selectedType.IsValueType));
            sb.AppendLine(string.Format("Is IsSerializable: {0}", selectedType.IsSerializable));
            sb.AppendLine(string.Format("Is IsSealed: {0}", selectedType.IsSealed));
            sb.AppendLine(string.Format("Is IsPublic: {0}", selectedType.IsPublic));
            sb.AppendLine(string.Format("Is IsNested: {0}", selectedType.IsNested));
            sb.AppendLine(string.Format("Is IsInterface: {0}", selectedType.IsInterface));
            sb.AppendLine(string.Format("Is IsGenericType: {0}", selectedType.IsGenericType));
            sb.AppendLine(string.Format("Is IsAbstract: {0}", selectedType.IsAbstract));
            lblInfo.Text=sb.ToString();
        }

        private void lstConstructor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConstructor.SelectedIndex == -1)
            {
                button1.Enabled= false;
            }
            else
            {
                button1.Enabled = true;
            }
            StringBuilder sb = new StringBuilder ();
            Type selectedType = assem.GetType ( cmbType.Text );
            ConstructorInfo[] ctors = selectedType.GetConstructors ();

            foreach ( ParameterInfo param in ctors[lstConstructor.SelectedIndex].GetParameters () )
            {
                sb.AppendLine ( ( string.Format ( "Parameter {0} is named {1} and is of type {2}", param.Position, param.Name, param.ParameterType ) ) );
            }
            if ( sb.ToString () == string.Empty )
            {
                lblInfo.Text = "This is a default constructor and doesn't have any parameters.";
            }
            else
            {
                lblInfo.Text = sb.ToString ();
            }
            
        }

        private void lstProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProperties.SelectedIndex==-1)
            {
                button4.Enabled = button5.Enabled = false;
            }
            else
            {
                button4.Enabled = button5.Enabled = true;
            }
            Type selectedType = assem.GetType(cmbType.Text);
            PropertyInfo pi = selectedType.GetProperty(lstProperties.Text);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Name: {0}", pi.Name));
            sb.AppendLine(string.Format("DeclaringType: {0}", pi.DeclaringType));
            sb.AppendLine(string.Format("Can Read: {0}", pi.CanRead));
            sb.AppendLine(string.Format("Can Write: {0}", pi.CanWrite));
            sb.AppendLine(string.Format("PropertyType: {0}", pi.PropertyType));
            lblInfo.Text = sb.ToString();


        }

        private void lstMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMethods.SelectedIndex == -1)
            {
                button6.Enabled = button2.Enabled = false;
            }
            else
            {
                button6.Enabled = button2.Enabled = true;
            }
            Type selectedType = assem.GetType(cmbType.Text);
            MethodInfo mi = selectedType.GetMethod(lstMethods.Text);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Name: {0}", mi.Name));
            sb.AppendLine(string.Format("Is Public: {0}", mi.IsPublic));
            sb.AppendLine(string.Format("Is Private: {0}", mi.IsPrivate));
            sb.AppendLine(string.Format("Is Static: {0}", mi.IsStatic));
            sb.AppendLine(string.Format("Is Virtual: {0}", mi.IsVirtual));
            sb.AppendLine(string.Format("Return Parameter: {0}", mi.ReturnParameter));
            sb.AppendLine(string.Format("Return Type: {0}", mi.ReturnType));
            sb.AppendLine(string.Format("Parameters:"));
            foreach (ParameterInfo item in mi.GetParameters())
            {
                sb.AppendLine(string.Format("Name: {0}", item.Name));
                sb.AppendLine(string.Format("Parameter Type: {0}", item.ParameterType));
                sb.AppendLine(string.Format("Is In: {0}", item.IsIn));
                sb.AppendLine(string.Format("Is Out: {0}", item.IsOut));
                sb.AppendLine(string.Format("Is Optional: {0}", item.IsOptional));
                sb.AppendLine(string.Format("Is Retval: {0}", item.IsRetval));
                sb.AppendLine(string.Format("Default Value: {0}", item.DefaultValue));
                sb.AppendLine();
            }

            lblInfo.Text = sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            object retVal = null;
            List<object> passingArgs = new List<object>();
            InputBox inputBox;

            selectedType = assem.GetType ( cmbType.Text );
            if ( selectedTypeInstance==null || selectedTypeInstance.GetType() != selectedType )
            {
                MessageBox.Show("No object exists of this type. Create an object first.");
                return;
            }

           
            MethodInfo methodInfo = selectedType.GetMethod ( lstMethods.Text );

            if ( selectedType.GetMethod ( lstMethods.Text ).GetParameters ().Count () == 0 )
            {
                
                retVal = methodInfo.Invoke ( selectedTypeInstance, null );
            }
            else
            {
                foreach ( ParameterInfo item in selectedType.GetMethod ( lstMethods.Text ).GetParameters () )
                {
                    
                    inputBox = new InputBox ();
                    inputBox.Prompt.Text = string.Format ( "Enter a value for ' {0} ' in {1}", item.Name, item.ParameterType );
                    inputBox.ShowDialog ();
                    
                    switch (item.ParameterType.Name  )
                    {

                        case "Int32":
                           
                            passingArgs.Add(Convert.ToInt32( inputBox.SelectedValue.Text));
                            break;

                        case  "String":
                             passingArgs.Add(Convert.ToString( inputBox.SelectedValue.Text));
                            break;

                        case "Double":
                             passingArgs.Add(Convert.ToDouble ( inputBox.SelectedValue.Text));
                            break;

                        case "Char":
                            passingArgs.Add(Convert.ToChar ( inputBox.SelectedValue.Text));
                            break;

                        default:
                             passingArgs.Add((object) inputBox.SelectedValue.Text);
                            break;
                    }
                    
                }
                retVal = methodInfo.Invoke ( selectedTypeInstance, passingArgs.ToArray<object>() );
            }




            if ( retVal == null )
            {
               lblInfo.Text=  "Return Value: Null" ;
            }
            else
            {
               lblInfo.Text= ( string.Format ( "Return Value: {0}", retVal.ToString () ) );
            }

        }
  
        private void lstOthers_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type selectedType = assem.GetType ( cmbType.Text );
            MemberInfo[]  mi = selectedType.GetMember  ( lstOthers.Text );
            StringBuilder sb = new StringBuilder ();
            sb.AppendLine ( string.Format ( "Name: {0}", mi[0].Name  ) );
            sb.AppendLine ( string.Format ( "Member Type: {0}", mi[0].MemberType  ) );
            lblInfo.Text = sb.ToString ();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedType = assem.GetType ( cmbType.Text );
            List<Type> constructorArgs =new List<Type>();
            List<object> passingArgs = new List<object> ();
            
            ConstructorInfo[] ctors = selectedType.GetConstructors ();

            foreach ( ParameterInfo param in ctors[lstConstructor.SelectedIndex].GetParameters () )
            {
               
               constructorArgs.Add( param.ParameterType);
            }
            ConstructorInfo ctor = selectedType.GetConstructor ( constructorArgs.ToArray<Type>() );

            if ( ctor.GetParameters ().Count () == 0 )
            {

                selectedTypeInstance = ctor.Invoke ( null );
            }
            else
            {
                InputBox inputBox;
                foreach ( ParameterInfo item in ctor.GetParameters () )
                {
                  
                    inputBox = new InputBox ();
                    inputBox.Prompt.Text = string.Format ( "Enter a value for ' {0} ' in {1}", item.Name, item.ParameterType );
                    inputBox.ShowDialog ();
                    switch (item.ParameterType.Name)
                    {

                        case "Int32":

                            passingArgs.Add(Convert.ToInt32(inputBox.SelectedValue.Text));
                            break;

                        case "String":
                            passingArgs.Add(Convert.ToString(inputBox.SelectedValue.Text));
                            break;

                        case "Double":
                            passingArgs.Add(Convert.ToDouble(inputBox.SelectedValue.Text));
                            break;

                        case "Char":
                            passingArgs.Add(Convert.ToChar(inputBox.SelectedValue.Text));
                            break;

                        default:
                            passingArgs.Add((object)inputBox.SelectedValue.Text);
                            break;
                    }
                  
                }
                selectedTypeInstance = ctor.Invoke ( passingArgs.ToArray<object> () );
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder ();
            Type selectedType = assem.GetType ( cmbType.Text );
            MethodInfo mi = selectedType.GetMethod ( lstMethods.Text  );
            MethodBody mb = mi.GetMethodBody ();
           sb.AppendLine (string.Format( "Method: {0}", mi.Name  ));
           sb.AppendLine ( string.Format ( "Local variables are initialized: {0}", mb.InitLocals ) );
           sb.AppendLine ( string.Format ( "Maximum number of items on the operand stack: {0}", mb.MaxStackSize ) );
           foreach ( LocalVariableInfo lvi in mb.LocalVariables )
           {
              sb.AppendLine(string.Format ( "Local variable: {0}", lvi ));
           }

           sb.AppendLine ();
           sb.AppendLine ( "The Machine Code of the Method: " );
          
           byte[] ilb = mb.GetILAsByteArray ();
           for ( int i = 0 ; i < ilb.Length ; i++ )
               sb.AppendLine(string.Format  ( "{0:X} ", ilb[i] ));

           lblInfo.Text = sb.ToString ();

         

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedTypeInstance == null || selectedTypeInstance.GetType() != selectedType)
            {
                MessageBox.Show("No object exists of this type. Create an object first.");
                return;
            }
            if ( !selectedType.GetProperty ( lstProperties.Text ).CanRead )
            {
                lblInfo.Text= "This property is Write-Only." ;
                return;
            }
            Type type = selectedTypeInstance.GetType ();
            PropertyInfo prop = type.GetProperty ( lstProperties.Text );
            object propertyValue= prop.GetValue  ( selectedTypeInstance,null);
            lblInfo.Text = string.Format ( "Property Value: {0}", propertyValue.ToString () );
        }


        private void Reset()
        {

            lstConstructor.Items.Clear();
            lstEvents.Items.Clear();
            lstMethods.Items.Clear();
            lstProperties.Items.Clear();
            cmbType.Items.Clear();
            cmbType.Text = string.Empty;
            selectedType = typeof(Int32);
            selectedTypeInstance = null;
            lblInfo.Text = string.Empty;
           button1.Enabled=button2.Enabled=button4.Enabled=button5.Enabled=button6.Enabled= btnGetInfo.Enabled = false;


        }


        private void button5_Click(object sender, EventArgs e)
        {

            selectedType = assem.GetType(cmbType.Text);
            if (selectedTypeInstance == null || selectedTypeInstance.GetType() != selectedType)
            {
                MessageBox.Show("No object exists of this type. Create an object first.");
                return;
            }

            if (! selectedType.GetProperty ( lstProperties.Text ).CanWrite  )
            {
                lblInfo.Text= "This property is Read-Only." ;
                return;
            }
            InputBox inputBox = new InputBox ();
            inputBox.Prompt.Text = string.Format ( "Enter a value for ' {0} ' in {1}", lstProperties.Text, selectedType.GetProperty ( lstProperties.Text ).PropertyType );
            inputBox.ShowDialog ();

            Type type = selectedTypeInstance.GetType ();
            PropertyInfo prop = type.GetProperty ( lstProperties.Text );
            switch (prop.PropertyType.Name )
            {
               case "Int32":

                            prop.SetValue ( selectedTypeInstance,Convert.ToInt32( inputBox.SelectedValue.Text), null );
                            break;

                        case "String":
                            prop.SetValue ( selectedTypeInstance,Convert.ToString( inputBox.SelectedValue.Text), null );
                            break;

                        case "Double":
                            prop.SetValue ( selectedTypeInstance,Convert.ToDouble( inputBox.SelectedValue.Text), null );
                            break;

                        case "Char":
                            prop.SetValue ( selectedTypeInstance,Convert.ToChar ( inputBox.SelectedValue.Text), null );
                            break;

                        default:
                            prop.SetValue ( selectedTypeInstance,(object) (inputBox.SelectedValue.Text), null );
                            break;
            }
           

            
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type selectedType = assem.GetType(cmbType.Text);
            EventInfo  ei = selectedType.GetEvent(lstEvents.Text);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Name: {0}", ei.Name));
            sb.AppendLine(string.Format("Is Multicast: {0}", ei.IsMulticast));
            sb.AppendLine(string.Format("Is SpecialName: {0}", ei.IsSpecialName));
            sb.AppendLine(string.Format("Raise Method: {0}",ei.GetRaiseMethod(true).Name ));
            sb.AppendLine(string.Format("Remove Method: {0}", ei.GetRemoveMethod().Name  ));

            lblInfo.Text = sb.ToString();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("My ResourceHacker by Morteza Gity");
        }

       

    }
}
