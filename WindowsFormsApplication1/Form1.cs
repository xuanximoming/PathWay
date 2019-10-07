using DrectSoft.Core;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");
        public static IDataAccess HISHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        public Form1()
        {
            InitializeComponent();
        }
    }
}
