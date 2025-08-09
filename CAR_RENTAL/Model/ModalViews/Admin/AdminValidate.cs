using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CAR_RENTAL.Model.ModalViews.Admin
{
    public class AdminValidate: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _phone;
        private bool _active;
        private bool _canCreate;

        public string Name
        {
            get { return _name; }
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropChanged();
                    ValidateForm();
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if( _email != value)
                {
                    _email = value; 
                    OnPropChanged();
                    ValidateForm();
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if(_password != value)
                {
                    _password = value;
                    OnPropChanged();
                    ValidateForm();
                }
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if(_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropChanged();
                    ValidateForm();
                }
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if ( _phone != value)
                {
                    _phone = value;
                    OnPropChanged();
                    ValidateForm();
                }
            }
        }
        public bool Active
        {
            get { return _active; }
            set
            {
                if( _active != value)
                {
                    _active = value;
                    OnPropChanged();
                }
            }
        }
        public bool CanCreate
        {
            get { return _canCreate; }
            set
            {
                if (_canCreate != value)
                {
                    _canCreate = value;
                    OnPropChanged();
                }
            }
        }

        
        private void ValidateForm()
        {
            CanCreate = !string.IsNullOrWhiteSpace(Name)
                        && !string.IsNullOrWhiteSpace(Email)
                        && !string.IsNullOrWhiteSpace(Password)
                        && Password == ConfirmPassword
                        && !string.IsNullOrWhiteSpace(Phone);
        }


    }
}
