using CleanBrain.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CleanBrain.UoF
{
    public class UnitOfWork : IDisposable
    {
        private Psychological_CenterEntities db = new Psychological_CenterEntities();
        private ClientRepository clientRepository;
        private PsychologistRepository psychologistRepository;
        private TimeTableRepository timeTableRepository;
        private DegreeRepository degreeRepository;
        private SpecializationRepository specializationRepository;
        private ReviewRepository reviewRepository;
        private ProcedureRepository procedureRepository;
        private VoucherRepository voucherRepository;
        private BookingRepository bookingRepository;

        public BookingRepository Booking
        {
            get
            {
                if (bookingRepository == null)
                    bookingRepository = new BookingRepository(db);
                return bookingRepository;
            }
        }
        public VoucherRepository Voucher
        {
            get
            {
                if (voucherRepository == null)
                    voucherRepository = new VoucherRepository(db);
                return voucherRepository;
            }
        }

        public ProcedureRepository Procedure
        {
            get
            {
                if (procedureRepository == null)
                    procedureRepository = new ProcedureRepository(db);
                return procedureRepository;
            }
        }
        public ReviewRepository Review
        {
            get
            {
                if (reviewRepository == null)
                    reviewRepository = new ReviewRepository(db);
                return reviewRepository;
            }
        }

        public SpecializationRepository Specialization
        {
            get
            {
                if (specializationRepository == null)
                    specializationRepository = new SpecializationRepository(db);
                return specializationRepository;
            }
        }
        public DegreeRepository Degree
        {
            get
            {
                if (degreeRepository == null)
                    degreeRepository = new DegreeRepository(db);
                return degreeRepository;
            }
        }
        public TimeTableRepository TimeTable
        {
            get
            {
                if (timeTableRepository == null)
                    timeTableRepository = new TimeTableRepository(db);
                return timeTableRepository;
            }
        }
        public PsychologistRepository Psychologist
        {
            get 
            {
                if (psychologistRepository == null)
                    psychologistRepository = new PsychologistRepository(db);
                return psychologistRepository;
            }
        }
        public ClientRepository Client
        {
            get
            { 
                if(clientRepository == null)
                    clientRepository = new ClientRepository(db);
                return clientRepository;
            }
        }
        public void Save()
        {
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    string entityName = entityValidationError.Entry.Entity.GetType().Name;
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        string propertyName = validationError.PropertyName;
                        string errorMessage = validationError.ErrorMessage;

                        MessageBox.Show($"Ошибка проверки сущности: {entityName}, Свойство: {propertyName}, Сообщение об ошибке: {errorMessage}");
                    }
                }
            }

        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing) 
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
