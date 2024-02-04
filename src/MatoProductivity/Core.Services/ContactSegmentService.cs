using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace MatoProductivity.Core.Services
{
    public class ContactSegmentService : NoteSegmentService, ITransientDependency
    {

        public Command PickContact { get; set; }
        private INoteSegmentPayload DefaultContactNameSegmentPayload => this.CreateNoteSegmentPayload(nameof(ContactName), "");
        private INoteSegmentPayload DefaultContactEmailSegmentPayload => this.CreateNoteSegmentPayload(nameof(ContactEmail), "");
        private INoteSegmentPayload DefaultContactPhoneSegmentPayload => this.CreateNoteSegmentPayload(nameof(ContactPhone), "");
        public ContactSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            this.PickContact = new Command(PickContactAction);
            PropertyChanged += ContactSegmentViewModel_PropertyChanged;
        }


        private void ContactSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Title), defaultTitle);
                Title = title.GetStringValue();

                var contactName = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(ContactName), DefaultContactNameSegmentPayload);
                ContactName = contactName.GetStringValue();


                var contactPhone = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(ContactPhone), DefaultContactPhoneSegmentPayload);
                ContactPhone = contactPhone.GetStringValue();


                var contactEmail = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(ContactEmail), DefaultContactEmailSegmentPayload);
                ContactEmail = contactEmail.GetStringValue();
            }

            else if (e.PropertyName == nameof(ContactPhone))
            {
                if (!string.IsNullOrEmpty(ContactPhone))
                {
                    NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(ContactPhone), ContactPhone));

                }
            }

            else if (e.PropertyName == nameof(ContactEmail))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(ContactEmail), ContactEmail));
            }
            else if (e.PropertyName == nameof(ContactName))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(ContactName), ContactName));
            }

            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }


        private async void PickContactAction(object obj)
        {
            if (await CheckPermissionIsGrantedAsync<ContactsRead>("联系人片段需要读取您设备的通讯录，请在设置中开启权限"))
            {
                try
                {
                    var contact = await Communication.Contacts.Default.PickContactAsync();

                    if (contact == null)
                        return;

                    string id = contact.Id;
                    string namePrefix = contact.NamePrefix;
                    string givenName = contact.GivenName;
                    string middleName = contact.MiddleName;
                    string familyName = contact.FamilyName;
                    string nameSuffix = contact.NameSuffix;
                    string displayName = contact.DisplayName;
                    List<ContactPhone> phones = contact.Phones;
                    List<ContactEmail> emails = contact.Emails;
                    this.ContactName = displayName;
                    this.ContactPhone = string.Join(',', phones);
                    this.ContactEmail = string.Join(',', emails);

                }
                catch (Exception ex)
                {
                    // Most likely permission denied
                }
            }
        }


        public override void CreateAction(object obj)
        {

        }

        private string _contactPhone;

        public string ContactPhone
        {
            get { return _contactPhone; }
            set
            {
                _contactPhone = value;
                RaisePropertyChanged();
            }
        }

        private string _contactName;

        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                RaisePropertyChanged();
            }
        }

        private string _contactEmail;

        public string ContactEmail
        {
            get { return _contactEmail; }
            set
            {
                _contactEmail = value;
                RaisePropertyChanged();
            }
        }




    }
}
