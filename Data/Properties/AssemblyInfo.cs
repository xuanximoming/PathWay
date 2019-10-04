//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright � Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Management.Instrumentation;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Resources;

[assembly : SecurityPermission(SecurityAction.RequestMinimum)]
[assembly : ReliabilityContract(Consistency.WillNotCorruptState, Cer.None)]
[assembly : AssemblyTitle("Enterprise Library Data Access Application Block")]
[assembly : AssemblyDescription("Enterprise Library Data Access Application Block")]
[assembly : AssemblyVersion("4.1.0.0")]
[assembly : Instrumented(@"root\EnterpriseLibrary")]
[assembly : WmiConfiguration(@"root\EnterpriseLibrary", HostingModel = ManagementHostingModel.Decoupled, IdentifyLevel = false)]
[assembly : AllowPartiallyTrustedCallers]
[assembly : SecurityTransparent]
[assembly: ComVisibleAttribute(false)]
[assembly: NeutralResourcesLanguageAttribute("zh-CN")]
