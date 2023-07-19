namespace WG3000_COMM.Basic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using WG3000_COMM.Core;
    using WG3000_COMM.DataOper;
    using WG3000_COMM.ExtendFunc.Elevator;
    using WG3000_COMM.Properties;
    using WG3000_COMM.ResStrings;

    partial class dfrmExtendedFunctions
    {
        private ImageButton btnCancel;
        private ImageButton btnOK;
        private ImageButton btnSetup;
        private CheckBox chkActivateAccessKeypad;
        private CheckBox chkActivateAntiPassBack;
        private CheckBox chkActivateControllerTaskList;
        private CheckBox chkActivateDontAutoLoadPrivileges;
        private CheckBox chkActivateDontAutoLoadSwipeRecords;
        private CheckBox chkActivateDontDisplayAccessControl;
        private CheckBox chkActivateDontDisplayAttendance;
        private CheckBox chkActivateDoorAsSwitch;
        private CheckBox chkActivateElevator;
        private CheckBox chkActivateFirstCardOpen;
        private CheckBox chkActivateInterLock;
        private CheckBox chkActivateMaps;
        private CheckBox chkActivateMeal;
        private CheckBox chkActivateMeeting;
        private CheckBox chkActivateMultiCardAccess;
        private CheckBox chkActivateOperatorManagement;
        private CheckBox chkActivateOtherShiftSchedule;
        private CheckBox chkActivatePatrol;
        private CheckBox chkActivatePCCheckAccess;
        private CheckBox chkActivatePeripheralControl;
        private CheckBox chkActivateRemoteOpenDoor;
        private CheckBox chkActivateTimeProfile;
        private CheckBox chkActivateTimeSegLimittedAccess;
        private CheckBox chkActivateWarnForceWithCard;
        private CheckBox chkActiveLogQuery;
        private CheckBox chkRecordButtonEvent;
        private CheckBox chkRecordDoorStatusEvent;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private new void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmExtendedFunctions));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkRecordDoorStatusEvent = new System.Windows.Forms.CheckBox();
            this.chkActiveLogQuery = new System.Windows.Forms.CheckBox();
            this.chkRecordButtonEvent = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkActivateDontDisplayQuickStart = new System.Windows.Forms.CheckBox();
            this.btnSetup = new System.Windows.Forms.ImageButton();
            this.chkActivatePatrol = new System.Windows.Forms.CheckBox();
            this.chkActivateMeal = new System.Windows.Forms.CheckBox();
            this.chkActivateMeeting = new System.Windows.Forms.CheckBox();
            this.chkActivateElevator = new System.Windows.Forms.CheckBox();
            this.chkActivateMaps = new System.Windows.Forms.CheckBox();
            this.chkActivateOtherShiftSchedule = new System.Windows.Forms.CheckBox();
            this.chkActivateDontDisplayAttendance = new System.Windows.Forms.CheckBox();
            this.chkActivateDontDisplayAccessControl = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkActivatePeripheralControl = new System.Windows.Forms.CheckBox();
            this.chkActivateAccessKeypad = new System.Windows.Forms.CheckBox();
            this.chkActivateRemoteOpenDoor = new System.Windows.Forms.CheckBox();
            this.chkActivateTimeProfile = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkActivateOperatorManagement = new System.Windows.Forms.CheckBox();
            this.chkActivateDoorAsSwitch = new System.Windows.Forms.CheckBox();
            this.chkActivateTimeSegLimittedAccess = new System.Windows.Forms.CheckBox();
            this.chkActivatePCCheckAccess = new System.Windows.Forms.CheckBox();
            this.chkActivateFirstCardOpen = new System.Windows.Forms.CheckBox();
            this.chkActivateMultiCardAccess = new System.Windows.Forms.CheckBox();
            this.chkActivateInterLock = new System.Windows.Forms.CheckBox();
            this.chkActivateAntiPassBack = new System.Windows.Forms.CheckBox();
            this.chkActivateControllerTaskList = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chkActivateDontAutoLoadSwipeRecords = new System.Windows.Forms.CheckBox();
            this.chkActivateDontAutoLoadPrivileges = new System.Windows.Forms.CheckBox();
            this.chkActivateWarnForceWithCard = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.ImageButton();
            this.btnCancel = new System.Windows.Forms.ImageButton();
            this.panelBottomBanner = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panelBottomBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkRecordDoorStatusEvent);
            this.groupBox1.Controls.Add(this.chkActiveLogQuery);
            this.groupBox1.Controls.Add(this.chkRecordButtonEvent);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkRecordDoorStatusEvent
            // 
            resources.ApplyResources(this.chkRecordDoorStatusEvent, "chkRecordDoorStatusEvent");
            this.chkRecordDoorStatusEvent.Name = "chkRecordDoorStatusEvent";
            this.chkRecordDoorStatusEvent.UseVisualStyleBackColor = true;
            // 
            // chkActiveLogQuery
            // 
            resources.ApplyResources(this.chkActiveLogQuery, "chkActiveLogQuery");
            this.chkActiveLogQuery.Name = "chkActiveLogQuery";
            this.chkActiveLogQuery.UseVisualStyleBackColor = true;
            // 
            // chkRecordButtonEvent
            // 
            resources.ApplyResources(this.chkRecordButtonEvent, "chkRecordButtonEvent");
            this.chkRecordButtonEvent.Name = "chkRecordButtonEvent";
            this.chkRecordButtonEvent.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.chkActivateDontDisplayQuickStart);
            this.groupBox2.Controls.Add(this.btnSetup);
            this.groupBox2.Controls.Add(this.chkActivatePatrol);
            this.groupBox2.Controls.Add(this.chkActivateMeal);
            this.groupBox2.Controls.Add(this.chkActivateMeeting);
            this.groupBox2.Controls.Add(this.chkActivateElevator);
            this.groupBox2.Controls.Add(this.chkActivateMaps);
            this.groupBox2.Controls.Add(this.chkActivateOtherShiftSchedule);
            this.groupBox2.Controls.Add(this.chkActivateDontDisplayAttendance);
            this.groupBox2.Controls.Add(this.chkActivateDontDisplayAccessControl);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkActivateDontDisplayQuickStart
            // 
            resources.ApplyResources(this.chkActivateDontDisplayQuickStart, "chkActivateDontDisplayQuickStart");
            this.chkActivateDontDisplayQuickStart.Name = "chkActivateDontDisplayQuickStart";
            this.chkActivateDontDisplayQuickStart.UseVisualStyleBackColor = true;
            // 
            // btnSetup
            // 
            this.btnSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            resources.ApplyResources(this.btnSetup, "btnSetup");
            this.btnSetup.Focusable = true;
            this.btnSetup.ForeColor = System.Drawing.Color.White;
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.TabStop = false;
            this.btnSetup.Toggle = false;
            this.btnSetup.UseVisualStyleBackColor = false;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // chkActivatePatrol
            // 
            resources.ApplyResources(this.chkActivatePatrol, "chkActivatePatrol");
            this.chkActivatePatrol.Name = "chkActivatePatrol";
            this.chkActivatePatrol.UseVisualStyleBackColor = true;
            // 
            // chkActivateMeal
            // 
            resources.ApplyResources(this.chkActivateMeal, "chkActivateMeal");
            this.chkActivateMeal.Name = "chkActivateMeal";
            this.chkActivateMeal.UseVisualStyleBackColor = true;
            // 
            // chkActivateMeeting
            // 
            resources.ApplyResources(this.chkActivateMeeting, "chkActivateMeeting");
            this.chkActivateMeeting.Name = "chkActivateMeeting";
            this.chkActivateMeeting.UseVisualStyleBackColor = true;
            // 
            // chkActivateElevator
            // 
            resources.ApplyResources(this.chkActivateElevator, "chkActivateElevator");
            this.chkActivateElevator.Name = "chkActivateElevator";
            this.chkActivateElevator.UseVisualStyleBackColor = true;
            this.chkActivateElevator.CheckedChanged += new System.EventHandler(this.chkActivateElevator_CheckedChanged);
            // 
            // chkActivateMaps
            // 
            resources.ApplyResources(this.chkActivateMaps, "chkActivateMaps");
            this.chkActivateMaps.Name = "chkActivateMaps";
            this.chkActivateMaps.UseVisualStyleBackColor = true;
            // 
            // chkActivateOtherShiftSchedule
            // 
            resources.ApplyResources(this.chkActivateOtherShiftSchedule, "chkActivateOtherShiftSchedule");
            this.chkActivateOtherShiftSchedule.Name = "chkActivateOtherShiftSchedule";
            this.chkActivateOtherShiftSchedule.UseVisualStyleBackColor = true;
            // 
            // chkActivateDontDisplayAttendance
            // 
            resources.ApplyResources(this.chkActivateDontDisplayAttendance, "chkActivateDontDisplayAttendance");
            this.chkActivateDontDisplayAttendance.Name = "chkActivateDontDisplayAttendance";
            this.chkActivateDontDisplayAttendance.UseVisualStyleBackColor = true;
            // 
            // chkActivateDontDisplayAccessControl
            // 
            resources.ApplyResources(this.chkActivateDontDisplayAccessControl, "chkActivateDontDisplayAccessControl");
            this.chkActivateDontDisplayAccessControl.Name = "chkActivateDontDisplayAccessControl";
            this.chkActivateDontDisplayAccessControl.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.chkActivatePeripheralControl);
            this.groupBox3.Controls.Add(this.chkActivateAccessKeypad);
            this.groupBox3.Controls.Add(this.chkActivateRemoteOpenDoor);
            this.groupBox3.Controls.Add(this.chkActivateTimeProfile);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkActivatePeripheralControl
            // 
            resources.ApplyResources(this.chkActivatePeripheralControl, "chkActivatePeripheralControl");
            this.chkActivatePeripheralControl.Name = "chkActivatePeripheralControl";
            this.chkActivatePeripheralControl.UseVisualStyleBackColor = true;
            // 
            // chkActivateAccessKeypad
            // 
            resources.ApplyResources(this.chkActivateAccessKeypad, "chkActivateAccessKeypad");
            this.chkActivateAccessKeypad.Name = "chkActivateAccessKeypad";
            this.chkActivateAccessKeypad.UseVisualStyleBackColor = true;
            // 
            // chkActivateRemoteOpenDoor
            // 
            resources.ApplyResources(this.chkActivateRemoteOpenDoor, "chkActivateRemoteOpenDoor");
            this.chkActivateRemoteOpenDoor.Name = "chkActivateRemoteOpenDoor";
            this.chkActivateRemoteOpenDoor.UseVisualStyleBackColor = true;
            // 
            // chkActivateTimeProfile
            // 
            resources.ApplyResources(this.chkActivateTimeProfile, "chkActivateTimeProfile");
            this.chkActivateTimeProfile.Name = "chkActivateTimeProfile";
            this.chkActivateTimeProfile.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.chkActivateOperatorManagement);
            this.groupBox4.Controls.Add(this.chkActivateDoorAsSwitch);
            this.groupBox4.Controls.Add(this.chkActivateTimeSegLimittedAccess);
            this.groupBox4.Controls.Add(this.chkActivatePCCheckAccess);
            this.groupBox4.Controls.Add(this.chkActivateFirstCardOpen);
            this.groupBox4.Controls.Add(this.chkActivateMultiCardAccess);
            this.groupBox4.Controls.Add(this.chkActivateInterLock);
            this.groupBox4.Controls.Add(this.chkActivateAntiPassBack);
            this.groupBox4.Controls.Add(this.chkActivateControllerTaskList);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // chkActivateOperatorManagement
            // 
            resources.ApplyResources(this.chkActivateOperatorManagement, "chkActivateOperatorManagement");
            this.chkActivateOperatorManagement.Name = "chkActivateOperatorManagement";
            this.chkActivateOperatorManagement.UseVisualStyleBackColor = true;
            // 
            // chkActivateDoorAsSwitch
            // 
            resources.ApplyResources(this.chkActivateDoorAsSwitch, "chkActivateDoorAsSwitch");
            this.chkActivateDoorAsSwitch.BackColor = System.Drawing.Color.Red;
            this.chkActivateDoorAsSwitch.Name = "chkActivateDoorAsSwitch";
            this.chkActivateDoorAsSwitch.UseVisualStyleBackColor = false;
            // 
            // chkActivateTimeSegLimittedAccess
            // 
            resources.ApplyResources(this.chkActivateTimeSegLimittedAccess, "chkActivateTimeSegLimittedAccess");
            this.chkActivateTimeSegLimittedAccess.BackColor = System.Drawing.Color.Red;
            this.chkActivateTimeSegLimittedAccess.Name = "chkActivateTimeSegLimittedAccess";
            this.chkActivateTimeSegLimittedAccess.UseVisualStyleBackColor = false;
            // 
            // chkActivatePCCheckAccess
            // 
            resources.ApplyResources(this.chkActivatePCCheckAccess, "chkActivatePCCheckAccess");
            this.chkActivatePCCheckAccess.Name = "chkActivatePCCheckAccess";
            this.chkActivatePCCheckAccess.UseVisualStyleBackColor = true;
            // 
            // chkActivateFirstCardOpen
            // 
            resources.ApplyResources(this.chkActivateFirstCardOpen, "chkActivateFirstCardOpen");
            this.chkActivateFirstCardOpen.Name = "chkActivateFirstCardOpen";
            this.chkActivateFirstCardOpen.UseVisualStyleBackColor = true;
            // 
            // chkActivateMultiCardAccess
            // 
            resources.ApplyResources(this.chkActivateMultiCardAccess, "chkActivateMultiCardAccess");
            this.chkActivateMultiCardAccess.Name = "chkActivateMultiCardAccess";
            this.chkActivateMultiCardAccess.UseVisualStyleBackColor = true;
            // 
            // chkActivateInterLock
            // 
            resources.ApplyResources(this.chkActivateInterLock, "chkActivateInterLock");
            this.chkActivateInterLock.Name = "chkActivateInterLock";
            this.chkActivateInterLock.UseVisualStyleBackColor = true;
            // 
            // chkActivateAntiPassBack
            // 
            resources.ApplyResources(this.chkActivateAntiPassBack, "chkActivateAntiPassBack");
            this.chkActivateAntiPassBack.Name = "chkActivateAntiPassBack";
            this.chkActivateAntiPassBack.UseVisualStyleBackColor = true;
            // 
            // chkActivateControllerTaskList
            // 
            resources.ApplyResources(this.chkActivateControllerTaskList, "chkActivateControllerTaskList");
            this.chkActivateControllerTaskList.Name = "chkActivateControllerTaskList";
            this.chkActivateControllerTaskList.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.chkActivateDontAutoLoadSwipeRecords);
            this.groupBox5.Controls.Add(this.chkActivateDontAutoLoadPrivileges);
            this.groupBox5.Controls.Add(this.chkActivateWarnForceWithCard);
            this.groupBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(81)))));
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // chkActivateDontAutoLoadSwipeRecords
            // 
            resources.ApplyResources(this.chkActivateDontAutoLoadSwipeRecords, "chkActivateDontAutoLoadSwipeRecords");
            this.chkActivateDontAutoLoadSwipeRecords.Name = "chkActivateDontAutoLoadSwipeRecords";
            this.chkActivateDontAutoLoadSwipeRecords.UseVisualStyleBackColor = true;
            // 
            // chkActivateDontAutoLoadPrivileges
            // 
            resources.ApplyResources(this.chkActivateDontAutoLoadPrivileges, "chkActivateDontAutoLoadPrivileges");
            this.chkActivateDontAutoLoadPrivileges.Name = "chkActivateDontAutoLoadPrivileges";
            this.chkActivateDontAutoLoadPrivileges.UseVisualStyleBackColor = true;
            // 
            // chkActivateWarnForceWithCard
            // 
            resources.ApplyResources(this.chkActivateWarnForceWithCard, "chkActivateWarnForceWithCard");
            this.chkActivateWarnForceWithCard.Name = "chkActivateWarnForceWithCard";
            this.chkActivateWarnForceWithCard.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnOK.Focusable = true;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Name = "btnOK";
            this.btnOK.Toggle = false;
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(124)))), ((int)(((byte)(170)))));
            this.btnCancel.Focusable = true;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Toggle = false;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panelBottomBanner
            // 
            this.panelBottomBanner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(101)))), ((int)(((byte)(155)))));
            this.panelBottomBanner.Controls.Add(this.btnCancel);
            this.panelBottomBanner.Controls.Add(this.btnOK);
            resources.ApplyResources(this.panelBottomBanner, "panelBottomBanner");
            this.panelBottomBanner.Name = "panelBottomBanner";
            // 
            // dfrmExtendedFunctions
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panelBottomBanner);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmExtendedFunctions";
            this.Load += new System.EventHandler(this.dfrmExtendedFunctions_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dfrmExtendedFunctions_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panelBottomBanner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private Panel panelBottomBanner;
        private CheckBox chkActivateDontDisplayQuickStart;
    }
}

