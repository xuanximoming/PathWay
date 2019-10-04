using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.JobManager
{
    public class SearchParameter
    {
        private string _searchText;
        /// <summary>
        /// �������ı�
        /// </summary>
        [Description("��־��Ŀ��Ϣ����������ı�(�����ִ�Сд)")]
        [Category("����")]
        [Browsable(true)]
        [DisplayName("��Ϣ�����ı�")]
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; }
        }

        private string _source;
        /// <summary>
        /// Դ
        /// </summary>
        [Description("��־��Ŀ��Դ")]
        [Category("����")]
        [Browsable(true)]
        [DisplayName("Դ")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        private string _user;
        /// <summary>
        /// �û�
        /// </summary>
        [DisplayName("�û�")]
        [Description("����־��Ŀ�������û�")]
        [Category("����")]
        [Browsable(true)]
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _computer;
        /// <summary>
        /// �����
        /// </summary>
        [DisplayName("�����")]
        [Description("����־��Ŀ�����ļ����")]
        [Category("����")]
        [Browsable(true)]
        public string Computer
        {
            get { return _computer; }
            set { _computer = value; }
        }

        private DateTime _startTime;
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        [DisplayName("��ʼʱ��")]
        [Description("��־��Ŀ�Ĵ���ʱ�䲻�����ڴ�����")]
        [Category("����")]
        [Browsable(true)]
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime _endTime;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [DisplayName("����ʱ��")]
        [Description("��־��Ŀ�Ĵ���ʱ�䲻�����ڴ�����")]
        [Category("����")]
        [Browsable(true)]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private bool _used;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        [Browsable(false)]
        public bool Used
        {
            get { return _used; }
            set { _used = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="source"></param>
        /// <param name="user"></param>
        /// <param name="computer"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public SearchParameter(string searchText, string source, string user, string computer
           , DateTime startTime, DateTime endTime, bool used)
        {
            _searchText = searchText;
            _source = source;
            _user = user;
            _computer = computer;
            _startTime = startTime;
            _endTime = endTime;
            _used = used;
        }

        public SearchParameter()
        { }

        /// <summary>
        /// �ж��Ƿ�Ϊ�յĲ���������ֵtrue��ʾ�ǿգ�false��ʾ�ղ���
        /// </summary>
        /// <returns></returns>
        public bool IsNotEmpty()
        {
            return !string.IsNullOrEmpty(this.Computer)
               || !string.IsNullOrEmpty(this.SearchText)
               || !string.IsNullOrEmpty(this.Source)
               || !string.IsNullOrEmpty(this.User)
               || !(this.StartTime == DateTime.MinValue)
               || !(this.EndTime == DateTime.MinValue
               || !(this.Used == false));
        }

        public SearchParameter Clone()
        {
            return new SearchParameter(this.SearchText, this.Source, this.User, this.Computer, this.StartTime, this.EndTime, this.Used);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SearchParameter))
                return false;
            SearchParameter parameter = obj as SearchParameter;
            if (parameter.Computer == this.Computer
               && parameter.EndTime == this.EndTime
               && parameter.SearchText == this.SearchText
               && parameter.Source == this.Source
               && parameter.StartTime == this.StartTime
               && parameter.User == this.User
               && parameter.Used == this.Used)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string temp = string.Empty;
            bool flagHead = false;
            if (!string.IsNullOrEmpty(_searchText))
            {
                temp += string.Format("������{0}��", _searchText);
                flagHead = true;
            }
            if (StartTime != DateTime.MinValue && EndTime != DateTime.MinValue)
            {
                temp += flagHead ? "��" : string.Empty;
                temp += string.Format("������{0}��{1}֮��", StartTime.ToString(), EndTime.ToString());
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_source))
            {
                temp += flagHead ? "��" : string.Empty;
                temp += string.Format("����Դ��{0}��", _source);
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_user))
            {
                temp += flagHead ? "��" : string.Empty;
                temp += string.Format("�û��ǡ�{0}��", _user);
                if (!flagHead)
                    flagHead = true;
            }
            if (!string.IsNullOrEmpty(_computer))
            {
                temp += flagHead ? "��" : string.Empty;
                temp += string.Format("������ǡ�{0}��", _computer);
                if (!flagHead)
                    flagHead = true;
            }
            if (flagHead)
                temp += "��";
            return temp;
        }
    }
}
