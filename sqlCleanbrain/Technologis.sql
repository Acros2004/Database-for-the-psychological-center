DROP PUBLIC DATABASE LINK CWCON;
CREATE PUBLIC DATABASE LINK CWCON
CONNECT TO CommonUser
IDENTIFIED BY Qwerty1111
USING '172.23.144.1:1521/CLEANBRAINMAINPC_PDB.be.by';
commit;

insert into Specialization@CWCON(Spezialization_Name) values('testinserta1');
UPDATE Specialization@CWCON SET Spezialization_Name = 'test' WHERE Spezialization_Name = 'testinserta';

drop trigger SpecializationTrigger;
CREATE OR REPLACE TRIGGER SpecializationTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Specialization
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Specialization@CWCON(Spezialization_Name) VALUES(:NEW.Spezialization_Name);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Spezialization_Name
    UPDATE Specialization@CWCON SET Spezialization_Name = :NEW.Spezialization_Name WHERE Spezialization_Name = :OLD.Spezialization_Name;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Spezialization_Name
    DELETE FROM Specialization@CWCON WHERE Spezialization_Name = :OLD.Spezialization_Name;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER AcademicDegreeTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Academic_Degree
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Academic_Degree@CWCON(Academic_Name) VALUES(:NEW.Academic_Name);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Academic_Name
    UPDATE Academic_Degree@CWCON SET Academic_Name = :NEW.Academic_Name WHERE Academic_Name = :OLD.Academic_Name;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Academic_Name
    DELETE FROM Academic_Degree@CWCON WHERE Academic_Name = :OLD.Academic_Name;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER VoucherTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Voucher
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Voucher@CWCON(Id_Voucher, Date_Voucher, Time_Voucher_Start, Time_Voucher_End, Id_Psychologist, Ordered)
    VALUES(:NEW.Id_Voucher, :NEW.Date_Voucher, :NEW.Time_Voucher_Start, :NEW.Time_Voucher_End, :NEW.Id_Psychologist, :NEW.Ordered);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Id_Voucher
    UPDATE Voucher@CWCON
    SET Date_Voucher = :NEW.Date_Voucher,
        Time_Voucher_Start = :NEW.Time_Voucher_Start,
        Time_Voucher_End = :NEW.Time_Voucher_End,
        Id_Psychologist = :NEW.Id_Psychologist,
        Ordered = :NEW.Ordered
    WHERE Id_Voucher = :OLD.Id_Voucher;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Id_Voucher
    DELETE FROM Voucher@CWCON WHERE Id_Voucher = :OLD.Id_Voucher;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER PsychologistTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Psychologist
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Psychologist@CWCON(Id_Psychologist, Name_Psychologist, Surname_Psychologist, Patronymic_Psychologist, Experience, Description, Spezialization_Psychologist, Degree)
    VALUES(:NEW.Id_Psychologist, :NEW.Name_Psychologist, :NEW.Surname_Psychologist, :NEW.Patronymic_Psychologist, :NEW.Experience, :NEW.Description, :NEW.Spezialization_Psychologist, :NEW.Degree);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Id_Psychologist
    UPDATE Psychologist@CWCON
    SET Name_Psychologist = :NEW.Name_Psychologist,
        Surname_Psychologist = :NEW.Surname_Psychologist,
        Patronymic_Psychologist = :NEW.Patronymic_Psychologist,
        Experience = :NEW.Experience,
        Description = :NEW.Description,
        Spezialization_Psychologist = :NEW.Spezialization_Psychologist,
        Degree = :NEW.Degree
    WHERE Id_Psychologist = :OLD.Id_Psychologist;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Id_Psychologist
    DELETE FROM Psychologist@CWCON WHERE Id_Psychologist = :OLD.Id_Psychologist;
  END IF;
END;
/


CREATE OR REPLACE TRIGGER TimetableTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Timetable
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Timetable@CWCON(Id_Psychologist, MondStart, MondEnd, TueStart, TueEnd, WenStart, WenEnd, ThuStart, ThuEnd, FriStart, FriEnd)
    VALUES(:NEW.Id_Psychologist, :NEW.MondStart, :NEW.MondEnd, :NEW.TueStart, :NEW.TueEnd, :NEW.WenStart, :NEW.WenEnd, :NEW.ThuStart, :NEW.ThuEnd, :NEW.FriStart, :NEW.FriEnd);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Id_Psychologist
    UPDATE Timetable@CWCON
    SET MondStart = :NEW.MondStart,
        MondEnd = :NEW.MondEnd,
        TueStart = :NEW.TueStart,
        TueEnd = :NEW.TueEnd,
        WenStart = :NEW.WenStart,
        WenEnd = :NEW.WenEnd,
        ThuStart = :NEW.ThuStart,
        ThuEnd = :NEW.ThuEnd,
        FriStart = :NEW.FriStart,
        FriEnd = :NEW.FriEnd
    WHERE Id_Psychologist = :OLD.Id_Psychologist;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Id_Psychologist
    DELETE FROM Timetable@CWCON WHERE Id_Psychologist = :OLD.Id_Psychologist;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER BookingTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Booking
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Booking@CWCON(ID_Booking, Date_Booking, Id_Procedure, Id_Client, Id_Psychologist, Id_Voucher, Time_Booking)
    VALUES(:NEW.ID_Booking, :NEW.Date_Booking, :NEW.Id_Procedure, :NEW.Id_Client, :NEW.Id_Psychologist, :NEW.Id_Voucher, :NEW.Time_Booking);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on ID_Booking
    UPDATE Booking@CWCON
    SET Date_Booking = :NEW.Date_Booking,
        Id_Procedure = :NEW.Id_Procedure,
        Id_Client = :NEW.Id_Client,
        Id_Psychologist = :NEW.Id_Psychologist,
        Id_Voucher = :NEW.Id_Voucher,
        Time_Booking = :NEW.Time_Booking
    WHERE ID_Booking = :OLD.ID_Booking;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on ID_Booking
    DELETE FROM Booking@CWCON WHERE ID_Booking = :OLD.ID_Booking;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER ReviewTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Review
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Review@CWCON(Id_Review, Name_Client, Id_Client, Review)
    VALUES(:NEW.Id_Review, :NEW.Name_Client, :NEW.Id_Client, :NEW.Review);
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Id_Review
    DELETE FROM Review@CWCON WHERE Id_Review = :OLD.Id_Review;
  END IF;
END;
/
drop trigger ClientTrigger;

CREATE OR REPLACE TRIGGER ClientTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Client
FOR EACH ROW
DECLARE
BEGIN
  IF (INSERTING) THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Client@CWCON(Id_client, Name_Client, Surname_Client, Login_Client, Password_Client, Mail_Client)
    VALUES(:NEW.Id_client, :NEW.Name_Client, :NEW.Surname_Client, :NEW.Login_Client, :NEW.Password_Client, :NEW.Mail_Client);
  END IF;

  IF (UPDATING) THEN
    -- Update data in the remote database based on Id_client
    UPDATE Client@CWCON
    SET Name_Client = :NEW.Name_Client,
        Surname_Client = :NEW.Surname_Client,
        Login_Client = :NEW.Login_Client,
        Password_Client = :NEW.Password_Client,
        Mail_Client = :NEW.Mail_Client
    WHERE Id_client = :OLD.Id_client;
  END IF;

  IF (DELETING) THEN
    -- Delete data from the remote database based on Id_client
    DELETE FROM Client@CWCON WHERE Id_client = :OLD.Id_client;
  END IF;
END;
/

CREATE OR REPLACE TRIGGER ProceduresTrigger
AFTER INSERT OR DELETE OR UPDATE
ON Procedures
FOR EACH ROW
DECLARE
BEGIN
  IF INSERTING THEN
    -- Insert data into the remote database using a database link
    INSERT INTO Procedures@CWCON(Id_Procedure, Name_Procedure, Price, Depiction, Spezialization_Procedure)
    VALUES(:NEW.Id_Procedure, :NEW.Name_Procedure, :NEW.Price, :NEW.Depiction, :NEW.Spezialization_Procedure);
  END IF;

  IF UPDATING THEN
    -- Update data in the remote database based on Id_Procedure
    UPDATE Procedures@CWCON
    SET Name_Procedure = :NEW.Name_Procedure,
        Price = :NEW.Price,
        Depiction = :NEW.Depiction,
        Spezialization_Procedure = :NEW.Spezialization_Procedure
    WHERE Id_Procedure = :OLD.Id_Procedure;
  END IF;

  IF DELETING THEN
    -- Delete data from the remote database based on Id_Procedure
    DELETE FROM Procedures@CWCON WHERE Id_Procedure = :OLD.Id_Procedure;
  END IF;
END;
/





