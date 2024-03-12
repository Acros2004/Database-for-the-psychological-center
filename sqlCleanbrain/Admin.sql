create role RLAdmin;
grant create session to RLAdmin;
grant execute on AdminPackage to RLAdmin;
grant restricted session to RLAdmin;
---------------------------------
DROP PUBLIC DATABASE LINK CWCON;
CREATE PUBLIC DATABASE LINK CWCON
CONNECT TO CommonUser
IDENTIFIED BY Qwerty1111
USING '172.22.32.1:1521/CLEANBRAINMAINPC_PDB.be.by';
commit;

insert into Specialization@CWCON(Spezialization_Name) values('testinserta');

create user AdminUser
IDENTIFIED by Qwerty12345;
grant RLAdmin to AdminUser;

create or replace package AdminPackage as
PROCEDURE GetClientById(p_Id_Client IN NUMBER,p_Cursor OUT SYS_REFCURSOR);

PROCEDURE InsertNewClient(p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Client_Id OUT NUMBER);

PROCEDURE AuthenticateClient(p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_AuthenticationResult OUT NUMBER);

PROCEDURE UpdateClientInfo(p_Id_Client IN NUMBER, p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllClients(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE CreateReview(p_Photo_Review IN BLOB, p_Name_Client IN VARCHAR2, p_Id_Client IN NUMBER, p_Review_Text IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE DeleteReviewById(p_Id IN NUMBER, p_Success OUT NUMBER);

PROCEDURE UpdateReview(p_Review_Id IN NUMBER, p_Photo_Review IN BLOB, p_Name_Client IN VARCHAR2, p_Review_Text IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllReviews(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE CreateTimeTable(p_Id_Psychologist IN NUMBER, p_MondStart IN TIMESTAMP, p_MondEnd IN TIMESTAMP, p_TueStart IN TIMESTAMP, p_TueEnd IN TIMESTAMP, p_WenStart IN TIMESTAMP, p_WenEnd IN TIMESTAMP, p_ThuStart IN TIMESTAMP, p_ThuEnd IN TIMESTAMP, p_FriStart IN TIMESTAMP, p_FriEnd IN TIMESTAMP, p_Result OUT NUMBER);

PROCEDURE GetTimeTableByPsychologistId(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE UpdateTimeTable(p_Id_Psychologist IN NUMBER, p_MondStart IN TIMESTAMP, p_MondEnd IN TIMESTAMP, p_TueStart IN TIMESTAMP, p_TueEnd IN TIMESTAMP, p_WenStart IN TIMESTAMP, p_WenEnd IN TIMESTAMP, p_ThuStart IN TIMESTAMP, p_ThuEnd IN TIMESTAMP, p_FriStart IN TIMESTAMP, p_FriEnd IN TIMESTAMP, p_Result OUT NUMBER);

PROCEDURE DeleteTimeTableById(p_Id_Psychologist IN NUMBER, p_Result OUT NUMBER);

PROCEDURE AddSpecialization(p_Spezialization_Name IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetSpecializationByName(p_SpecializationName IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE AddDegree(p_Degree_Name IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetDegreeByName(p_Degree_Name IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE AddProcedure(p_Name_Procedure IN NVARCHAR2, p_Price IN FLOAT, p_Depiction IN NVARCHAR2, p_Spezialization_Procedure IN NVARCHAR2, p_Photo_Procedure IN BLOB, p_Result OUT NUMBER);

PROCEDURE DeleteProcedure(p_Id_Procedure IN NUMBER, p_Result OUT NUMBER);

PROCEDURE UpdateProcedure(p_Id_Procedure IN NUMBER, p_Name_Procedure IN NVARCHAR2, p_Price IN FLOAT, p_Depiction IN NVARCHAR2, p_Spezialization_Procedure IN NVARCHAR2, p_Photo_Procedure IN BLOB, p_Result OUT NUMBER);

PROCEDURE GetProcedureById(p_Procedure_Id IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllProcedures(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE AddVoucher(p_Date_Voucher IN DATE, p_Time_Voucher_Start IN TIMESTAMP, p_Time_Voucher_End IN TIMESTAMP, p_Id_Psychologist IN NUMBER, p_Ordered IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetVoucherById(p_Id_Voucher IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllVouchers(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE DeleteVoucher(p_Id_Voucher IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteOldVouchers(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteNotOrdered(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteAllById(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE UpdateVoucher(p_Id_Voucher IN NUMBER, p_Date_Voucher IN DATE, p_Time_Voucher_Start IN TIMESTAMP, p_Time_Voucher_End IN TIMESTAMP, p_Id_Psychologist IN NUMBER, p_Ordered IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE CreateBooking(p_Date_Booking IN DATE, p_Id_Procedure IN NUMBER, p_Id_Client IN NUMBER, p_Id_Psychologist IN NUMBER, p_Id_Voucher IN NUMBER, p_Time_Booking IN TIMESTAMP, p_Result OUT NUMBER);

PROCEDURE GetBookingById(p_BookingId IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllBookings(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE DeleteBooking(p_BookingId IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteBookingByIdPsy(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE AddPsychologist(p_Name_Psychologist IN NVARCHAR2, p_Surname_Psychologist IN NVARCHAR2, p_Patronymic_Psychologist IN NVARCHAR2, p_Experience IN NUMBER, p_Photo_Psychologist IN BLOB, p_Description IN NVARCHAR2, p_Specialization_Psychologist IN NVARCHAR2, p_Degree IN NVARCHAR2, p_Result OUT NUMBER);

PROCEDURE UpdatePsychologist(p_Id_Psychologist IN NUMBER, p_Name_Psychologist IN NVARCHAR2, p_Surname_Psychologist IN NVARCHAR2, p_Patronymic_Psychologist IN NVARCHAR2, p_Experience IN NUMBER, p_Photo_Psychologist IN BLOB, p_Description IN NVARCHAR2, p_Spezialization_Psychologist IN NVARCHAR2, p_Degree IN NVARCHAR2, p_Result OUT NUMBER);

PROCEDURE DeletePsychologist(p_Id_Psychologist IN NUMBER, p_Result OUT NUMBER);

PROCEDURE GetPsychologistById(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllPsychologists(p_Cursor OUT SYS_REFCURSOR);
END AdminPackage;
/
CREATE OR REPLACE PACKAGE BODY AdminPackage AS

PROCEDURE GetClientById(
    p_Id_Client IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client
        WHERE Id_client = p_Id_Client;
END GetClientById;

PROCEDURE InsertNewClient(
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Client_Id OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientData(p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- ���� ������ ������ ���������, ��������� ������ ������������
    IF v_Valid THEN
        INSERT INTO Client (Name_Client, Surname_Client, Login_Client, Password_Client, Photo_Client, Mail_Client)
        VALUES (p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
        COMMIT; -- �������� ��������� � ���� ������
        
        
        SELECT Id_client
        INTO p_Client_Id FROM Client
        WHERE Login_Client = p_Login_Client; 
    ELSE
        p_Client_Id := -1; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        rollback;
        p_Client_Id := -1;
END InsertNewClient;

PROCEDURE AuthenticateClient(
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_AuthenticationResult OUT NUMBER
)
IS
    v_ValidData BOOLEAN := ValidateAuthenticationData(p_Login_Client, p_Password_Client);
    v_Client_Count NUMBER;
    v_Client_id NUMBER;
    pass Client.Password_Client%Type;
BEGIN
    -- �������� ���������� ������
    pass := EncryptPassword(p_Password_Client);
    IF v_ValidData THEN
        SELECT COUNT(*)
        INTO v_Client_Count
        FROM Client
        WHERE Login_Client = p_Login_Client AND Password_Client = pass;
        IF v_Client_Count > 0 THEN
            SELECT Id_client
            INTO v_Client_id
            FROM Client
            WHERE Login_Client = p_Login_Client AND Password_Client = pass;

            p_AuthenticationResult := v_Client_id; -- �������� ��������������
        ELSE
            p_AuthenticationResult := -5; -- ��������� ��������������
        END IF;
    ELSE
        p_AuthenticationResult := -20; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        p_AuthenticationResult := -1; -- ������ � �������� ����������
END AuthenticateClient;

PROCEDURE UpdateClientInfo(
    p_Id_Client IN NUMBER,
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientUpdateData(p_Name_Client, p_Surname_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � �������
        UPDATE Client
        SET
            Name_Client = p_Name_Client,
            Surname_Client = p_Surname_Client,
            Photo_Client = p_Photo_Client,
            Mail_Client = p_Mail_Client
        WHERE Id_Client = p_Id_Client;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ����������
    ELSE
        p_Result := 0; -- ������ �� ���� ��������� (��������, ������ � ��������� Id_Client �� ������)
    END IF;
EXCEPTION
    WHEN OTHERS THEN
        rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateClientInfo;

PROCEDURE GetAllClients(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client;
END GetAllClients;

--Review

PROCEDURE CreateReview(
    p_Photo_Review in BLOB,
    p_Name_Client IN VARCHAR2,
    p_Id_Client IN NUMBER,
    p_Review_Text IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateReviewData(p_Review_Text);
    v_Client_Count NUMBER;
    v_Previous_Review_Count NUMBER;
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- �������� ������������� ������������ �� Id_Client
        SELECT COUNT(*)
        INTO v_Client_Count
        FROM Client
        WHERE Id_Client = p_Id_Client;

        IF v_Client_Count > 0 THEN
            SELECT COUNT(*)
            INTO v_Previous_Review_Count
            FROM Review
            WHERE Id_Client = p_Id_Client;

            IF v_Previous_Review_Count = 0 THEN
                -- ������� ������ � ������� Review
                INSERT INTO Review (Photo_Review, Name_Client, Id_Client, Review)
                VALUES (p_Photo_Review, p_Name_Client, p_Id_Client, p_Review_Text);
    
                COMMIT; -- �������� ��������� � ���� ������
                p_Result := 1; -- �������� �������� ������
            ELSE
                p_Result := 0;
            END IF;
        ELSE
            p_Result := 0; -- ������������ � ��������� Id_Client �� ����������
        END IF;
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END CreateReview;

PROCEDURE DeleteReviewById(
    p_Id IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    DELETE FROM Review
    WHERE Id_Review = p_Id;

    IF SQL%ROWCOUNT > 0 THEN
        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END IF;
END DeleteReviewById;

PROCEDURE UpdateReview(
    p_Review_Id IN NUMBER,
    p_Photo_Review IN BLOB,
    p_Name_Client IN VARCHAR2,
    p_Review_Text IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateReviewData(p_Review_Text);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � ������
        UPDATE Review
        SET 
            Photo_Review = p_Photo_Review,
            Name_Client = p_Name_Client,
            Review = p_Review_Text
        WHERE Id_Review = p_Review_Id;

        -- �������� ���������� ����������� �����
        IF SQL%ROWCOUNT > 0 THEN
            p_Result := 1; -- �������� ����������
            COMMIT; -- �������� ��������� � ���� ������
        ELSE
            p_Result := 0; -- �� ������� �������� ����� (��������, ����� � ��������� Id_Review �� ������)
        END IF;
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateReview;


PROCEDURE GetAllReviews(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Review;
END GetAllReviews;
-- Timetable

PROCEDURE CreateTimeTable(
    p_Id_Psychologist IN NUMBER,
    p_MondStart IN TIMESTAMP ,
    p_MondEnd IN TIMESTAMP,
    p_TueStart IN TIMESTAMP,
    p_TueEnd IN TIMESTAMP,
    p_WenStart IN TIMESTAMP,
    p_WenEnd IN TIMESTAMP,
    p_ThuStart IN TIMESTAMP,
    p_ThuEnd IN TIMESTAMP,
    p_FriStart IN TIMESTAMP,
    p_FriEnd IN TIMESTAMP,
    p_Result OUT NUMBER
)
IS
    v_Psychologist_Count NUMBER;
BEGIN
    -- �������� ������������� ��������� �� Id_Psychologist
    SELECT COUNT(*)
    INTO v_Psychologist_Count
    FROM Timetable
    WHERE Id_Psychologist = p_Id_Psychologist;

    IF v_Psychologist_Count = 0 THEN
        INSERT INTO Timetable (
            Id_Psychologist,
            MondStart, MondEnd,
            TueStart, TueEnd,
            WenStart, WenEnd,
            ThuStart, ThuEnd,
            FriStart, FriEnd
            ) VALUES (
            p_Id_Psychologist,
            p_MondStart, p_MondEnd,
            p_TueStart, p_TueEnd,
            p_WenStart, p_WenEnd,
            p_ThuStart, p_ThuEnd,
            p_FriStart, p_FriEnd
            );

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� �������� ����������
    ELSE
        p_Result := 0; -- �������� � ��������� Id_Psychologist �� ����������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END CreateTimeTable;

PROCEDURE GetTimeTableByPsychologistId(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Timetable
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetTimeTableByPsychologistId;

PROCEDURE UpdateTimeTable(
    p_Id_Psychologist IN NUMBER,
    p_MondStart IN TIMESTAMP,
    p_MondEnd IN TIMESTAMP,
    p_TueStart IN TIMESTAMP,
    p_TueEnd IN TIMESTAMP,
    p_WenStart IN TIMESTAMP,
    p_WenEnd IN TIMESTAMP,
    p_ThuStart IN TIMESTAMP,
    p_ThuEnd IN TIMESTAMP,
    p_FriStart IN TIMESTAMP,
    p_FriEnd IN TIMESTAMP,
    p_Result OUT NUMBER
)
IS
BEGIN
    -- �������� ������������� ������ � ������� Timetable
    SELECT COUNT(*)
    INTO p_Result
    FROM Timetable
    WHERE Id_Psychologist = p_Id_Psychologist;

    IF p_Result > 0 THEN
        -- ���������� ���������� � ����������
        UPDATE Timetable
        SET
            MondStart = p_MondStart,
            MondEnd = p_MondEnd,
            TueStart = p_TueStart,
            TueEnd = p_TueEnd,
            WenStart = p_WenStart,
            WenEnd = p_WenEnd,
            ThuStart = p_ThuStart,
            ThuEnd = p_ThuEnd,
            FriStart = p_FriStart,
            FriEnd = p_FriEnd
        WHERE Id_Psychologist = p_Id_Psychologist;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ����������
    ELSE
        p_Result := 0; -- ������ � ������� Timetable �� �������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateTimeTable;

PROCEDURE DeleteTimeTableById(
    p_Id_Psychologist IN NUMBER,
    p_Result OUT NUMBER
)
IS
BEGIN
    DELETE FROM Timetable
    WHERE Id_Psychologist = p_Id_Psychologist;
    
    -- �������� �� ���������� ��������� �������
    IF SQL%ROWCOUNT > 0 THEN
        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ��������
    ELSE
        p_Result := 0; -- ������ � ������� Timetable �� �������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END DeleteTimeTableById;

--Spec

PROCEDURE AddSpecialization(
    p_Spezialization_Name IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
 v_Valid BOOLEAN := ValidateSpecializationData(p_Spezialization_Name);
BEGIN
    -- ����� ������� ��������� ������
    IF v_Valid THEN
        INSERT INTO Specialization (Spezialization_Name)
        VALUES (p_Spezialization_Name);
        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� �������������
    ELSE
        p_Result := 0;
    END IF;        
    EXCEPTION
        WHEN OTHERS THEN
        rollback;
            p_Result := 0; -- ������ � �������� ����������
END AddSpecialization;

PROCEDURE GetSpecializationByName(
    p_SpecializationName IN VARCHAR2,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Specialization
        WHERE Spezialization_Name = p_SpecializationName;
END GetSpecializationByName;
--degree
PROCEDURE AddDegree(
    p_Degree_Name IN VARCHAR2,    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateDegreeData(p_Degree_Name);
BEGIN
    -- ����� ������� ��������� ������
    IF v_Valid THEN
        INSERT INTO Academic_Degree (Academic_Name)
        VALUES (p_Degree_Name);
        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� �������
    ELSE
        p_Result := 0;
    END IF;
    EXCEPTION
        WHEN OTHERS THEN
        rollback;
            p_Result := 0; -- ������ � �������� ����������
END AddDegree;

PROCEDURE GetDegreeByName(
    p_Degree_Name IN VARCHAR2,    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Academic_Degree
        WHERE Academic_Name = p_Degree_Name;
END GetDegreeByName;
--proc
PROCEDURE AddProcedure(
    p_Name_Procedure IN NVARCHAR2,
    p_Price IN FLOAT,
    p_Depiction IN NVARCHAR2,
    p_Spezialization_Procedure IN NVARCHAR2,
    p_Photo_Procedure IN BLOB,
    p_Result OUT NUMBER
)
AS
    v_Count NUMBER;
    v_ValidInput BOOLEAN;
BEGIN
    -- �������� ������� ������ �� ����������
    v_ValidInput := IsValidInput(p_Name_Procedure, p_Price, p_Depiction);

    IF v_ValidInput THEN
        SELECT COUNT(*) INTO v_Count FROM Procedures WHERE Name_Procedure = p_Name_Procedure;
        IF v_Count > 0 THEN
            p_Result := 0;  -- ������ � ����� ��������� ��� ����������
        ELSE
            INSERT INTO Procedures (Name_Procedure, Price, Depiction, Spezialization_Procedure, Photo_Procedure)
            VALUES (p_Name_Procedure, p_Price, p_Depiction, p_Spezialization_Procedure, p_Photo_Procedure);
            COMMIT;
            p_Result := 1;  -- �������� ������� ������
        END IF;
    ELSE
        p_Result := 0;
    END IF;
    EXCEPTION
        WHEN OTHERS THEN
        rollback;
            p_Result := 0;  -- � ������ ������ ������������� p_Result � 0
END AddProcedure;

PROCEDURE DeleteProcedure(
    p_Id_Procedure IN NUMBER,
    p_Result OUT NUMBER
)
AS
BEGIN
    DELETE FROM Procedures WHERE Id_Procedure = p_Id_Procedure;
    COMMIT;
    p_Result := SQL%ROWCOUNT; -- ���������� ���������� ��������� �������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END DeleteProcedure;

PROCEDURE UpdateProcedure(
    p_Id_Procedure IN NUMBER,
    p_Name_Procedure IN NVARCHAR2,
    p_Price IN FLOAT,
    p_Depiction IN NVARCHAR2,
    p_Spezialization_Procedure IN NVARCHAR2,
    p_Photo_Procedure IN BLOB,
    p_Result OUT NUMBER
)
AS
    v_Count NUMBER;
    v_ValidInput BOOLEAN;
BEGIN
    -- �������� ������� ������ �� ����������
    v_ValidInput := IsValidInput(p_Name_Procedure, p_Price, p_Depiction);

    IF v_ValidInput THEN
        SELECT COUNT(*) INTO v_Count FROM Procedures WHERE Id_Procedure = p_Id_Procedure;
        IF v_Count = 0 THEN
            p_Result := 0;
        ELSE
            UPDATE Procedures
            SET
            Name_Procedure = p_Name_Procedure,
            Price = p_Price,
            Depiction = p_Depiction,
            Spezialization_Procedure = p_Spezialization_Procedure,
            Photo_Procedure = p_Photo_Procedure
            WHERE Id_Procedure = p_Id_Procedure;
            
            COMMIT;
            p_Result := 1;  -- �������� ����������
        END IF;
    ELSE
        p_Result := 0;  -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0;  -- � ������ ������ ������������� p_Result � 0
END UpdateProcedure;

PROCEDURE GetProcedureById(
    p_Procedure_Id IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures
        WHERE Id_Procedure = p_Procedure_Id;
END GetProcedureById;

PROCEDURE GetAllProcedures(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures;
END GetAllProcedures;
--voucher

PROCEDURE AddVoucher(
    p_Date_Voucher IN DATE,
    p_Time_Voucher_Start IN TIMESTAMP,
    p_Time_Voucher_End IN TIMESTAMP,
    p_Id_Psychologist IN NUMBER,
    p_Ordered IN VARCHAR2,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� �������� ���� ordered
    IF UPPER(p_Ordered) IN ('��', '���') THEN
        INSERT INTO Voucher (Date_Voucher,Time_Voucher_Start,Time_Voucher_End,Id_Psychologist,Ordered)
        VALUES (p_Date_Voucher, p_Time_Voucher_Start, p_Time_Voucher_End,p_Id_Psychologist, p_Ordered);
        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� �������
    ELSE
        p_Result := 0; -- �������� �������� ���� ordered
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END AddVoucher;

PROCEDURE GetVoucherById(
    p_Id_Voucher IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Voucher
        WHERE Id_Voucher = p_Id_Voucher;
END GetVoucherById;

PROCEDURE GetAllVouchers(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Voucher;
END GetAllVouchers;

PROCEDURE DeleteVoucher(
    p_Id_Voucher IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    DELETE FROM Voucher
    WHERE Id_Voucher = p_Id_Voucher;

    IF SQL%ROWCOUNT > 0 THEN
        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END IF;
END DeleteVoucher;

PROCEDURE DeleteOldVouchers(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    -- ������ ��� ������, ��������������� ��������
    FOR voucher_rec IN (SELECT *
                        FROM Voucher
                        WHERE Id_Psychologist = p_Id_Psychologist
                          AND Date_Voucher < SYSTIMESTAMP) LOOP

        -- �������� ������� ��� ��������
        IF voucher_rec.Ordered = '���' THEN
            -- �������� ������
            DELETE FROM Voucher
            WHERE Id_Voucher = voucher_rec.Id_Voucher;
        ELSE
            -- �������� ��������� ������������
            FOR booking_rec IN (SELECT *
                                FROM Booking
                                WHERE Id_Psychologist = p_Id_Psychologist
                                  AND Id_Voucher = voucher_rec.Id_Voucher) LOOP

                DELETE FROM Booking
                WHERE Id_Booking = booking_rec.Id_Booking;

            END LOOP;

            -- �������� ������
            DELETE FROM Voucher
            WHERE Id_Voucher = voucher_rec.Id_Voucher;

        END IF;

        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END LOOP;

END DeleteOldVouchers;

PROCEDURE DeleteNotOrdered(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    -- �������� ������� � Ordered = '���'
    FOR voucher IN (SELECT * FROM Voucher WHERE Ordered = '���' AND Id_Psychologist = p_Id_Psychologist)
    LOOP
        DELETE FROM Voucher WHERE Id_Voucher = voucher.Id_Voucher;
    END LOOP;

    COMMIT; -- �������� ��������� � ���� ������

    p_Success := 1; -- �������� ��������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Success := 0; -- ������ � �������� ����������
END DeleteNotOrdered;

PROCEDURE DeleteAllById(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    -- �������� ���� ������� �� Id_Psychologist
    FOR voucher IN (SELECT * FROM Voucher WHERE Id_Psychologist = p_Id_Psychologist)
    LOOP
        DELETE FROM Voucher WHERE Id_Voucher = voucher.Id_Voucher;
    END LOOP;

    COMMIT; -- �������� ��������� � ���� ������

    p_Success := 1; -- �������� ��������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Success := 0; -- ������ � �������� ����������
END DeleteAllById;

PROCEDURE UpdateVoucher(
    p_Id_Voucher IN NUMBER,
    p_Date_Voucher IN DATE,
    p_Time_Voucher_Start IN TIMESTAMP,
    p_Time_Voucher_End IN TIMESTAMP,
    p_Id_Psychologist IN NUMBER,
    p_Ordered IN VARCHAR2,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� �������� ���� ordered
    IF UPPER(p_Ordered) IN ('��', '���') THEN
        -- ���������� ���������� � ������
        UPDATE Voucher
        SET
            Date_Voucher = p_Date_Voucher,
            Time_Voucher_Start = p_Time_Voucher_Start,
            Time_Voucher_End = p_Time_Voucher_End,
            Id_Psychologist = p_Id_Psychologist,
            Ordered = p_Ordered
        WHERE Id_Voucher = p_Id_Voucher;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� ������
    ELSE
        p_Result := 0; -- �������� �������� ���� ordered
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateVoucher;
--booking
PROCEDURE CreateBooking(
    p_Date_Booking IN DATE,
    p_Id_Procedure IN NUMBER,
    p_Id_Client IN NUMBER,
    p_Id_Psychologist IN NUMBER,
    p_Id_Voucher IN NUMBER,
    p_Time_Booking IN TIMESTAMP,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� ������� ���������� ������� � ��������� Id_Voucher
    DECLARE
        v_Voucher_Count NUMBER;
    BEGIN
        SELECT COUNT(*)
        INTO v_Voucher_Count
        FROM Voucher
        WHERE Id_Voucher = p_Id_Voucher AND Ordered = '���';

        IF v_Voucher_Count = 0 THEN
            -- ������ ����� ��� �� ����������
            p_Result := 0;
            RETURN;
        END IF;
    END;

    -- �������� ������������
    INSERT INTO Booking (
    Date_Booking,
    Id_Procedure,
    Id_Client,
    Id_Psychologist,
    Id_Voucher,
    Time_Booking
    ) VALUES (
    p_Date_Booking,
    p_Id_Procedure,
    p_Id_Client,
    p_Id_Psychologist,
    p_Id_Voucher,
    p_Time_Booking
    );

    COMMIT; -- �������� ��������� � ���� ������
    p_Result := 1; -- �������� �������� ������������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END CreateBooking;

PROCEDURE GetBookingById(
    p_BookingId IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Booking
        WHERE Id_Booking = p_BookingId;
END GetBookingById;

PROCEDURE GetAllBookings(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Booking;
END GetAllBookings;

PROCEDURE DeleteBooking(
    p_BookingId IN NUMBER,
    p_Success OUT NUMBER
)
AS
BEGIN
    -- ������� �������� ������
    DELETE FROM Booking
    WHERE Id_Booking = p_BookingId;

    IF SQL%ROWCOUNT > 0 THEN
        -- �������� ��������
        COMMIT;
        p_Success := 1;
    ELSE
        -- ������ �� �������
        p_Success := 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBooking;

PROCEDURE DeleteBookingByIdPsy(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
AS
BEGIN
    -- ������� �������� �������
    DELETE FROM Booking
    WHERE Id_Psychologist = p_Id_Psychologist;

    IF SQL%ROWCOUNT > 0 THEN
        -- �������� ��������
        COMMIT;
        p_Success := 1;
    ELSE
        -- ������ �� �������
        p_Success := 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBookingByIdPsy;
--psy
PROCEDURE AddPsychologist(
    p_Name_Psychologist IN NVARCHAR2,
    p_Surname_Psychologist IN NVARCHAR2,
    p_Patronymic_Psychologist IN NVARCHAR2,
    p_Experience IN NUMBER,
    p_Photo_Psychologist IN BLOB,
    p_Description IN NVARCHAR2,
    p_Specialization_Psychologist IN NVARCHAR2,
    p_Degree IN NVARCHAR2,
    p_Result OUT NUMBER
)
AS
    v_Valid BOOLEAN := ValidatePsychologistData(
        p_Name_Psychologist,
        p_Surname_Psychologist,
        p_Patronymic_Psychologist,
        p_Description
    );
BEGIN
    -- ����� ������� ��������� ������
    IF v_Valid THEN
        INSERT INTO Psychologist (
        Name_Psychologist,
        Surname_Psychologist,
        Patronymic_Psychologist,
        Experience,
        Photo_Psychologist,
        Description,
        Spezialization_Psychologist,
        Degree
    ) VALUES (
        p_Name_Psychologist,
        p_Surname_Psychologist,
        p_Patronymic_Psychologist,
        p_Experience,
        p_Photo_Psychologist,
        p_Description,
        p_Specialization_Psychologist,
        p_Degree
    );
        commit;
        p_Result := 1; -- �������� ���������� ���������
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        -- ������ � �������� ����������
        p_Result := 0;
        Raise;
END AddPsychologist;

PROCEDURE UpdatePsychologist(
    p_Id_Psychologist IN NUMBER,
    p_Name_Psychologist IN NVARCHAR2,
    p_Surname_Psychologist IN NVARCHAR2,
    p_Patronymic_Psychologist IN NVARCHAR2,
    p_Experience IN NUMBER,
    p_Photo_Psychologist IN BLOB,
    p_Description IN NVARCHAR2,
    p_Spezialization_Psychologist IN NVARCHAR2,
    p_Degree IN NVARCHAR2,
    p_Result OUT NUMBER
)
AS
    v_Valid BOOLEAN := ValidatePsychologistData(p_Name_Psychologist, p_Surname_Psychologist, p_Patronymic_Psychologist, p_Description);

BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � ���������
        UPDATE Psychologist
        SET
            Name_Psychologist = p_Name_Psychologist,
            Surname_Psychologist = p_Surname_Psychologist,
            Patronymic_Psychologist = p_Patronymic_Psychologist,
            Experience = p_Experience,
            Photo_Psychologist = p_Photo_Psychologist,
            Description = p_Description,
            Spezialization_Psychologist = p_Spezialization_Psychologist,
            Degree = p_Degree
        WHERE Id_Psychologist = p_Id_Psychologist;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ����������
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdatePsychologist;

PROCEDURE DeletePsychologist(
    p_Id_Psychologist IN NUMBER,
    p_Result OUT NUMBER
)
AS
BEGIN
    DELETE FROM Psychologist WHERE Id_Psychologist = p_Id_Psychologist;
    COMMIT;
    p_Result := SQL%ROWCOUNT; -- ���������� ���������� ��������� �������
EXCEPTION
    WHEN OTHERS THEN
        p_Result := 0; -- ������ � �������� ����������
END DeletePsychologist;

PROCEDURE GetPsychologistById(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetPsychologistById;

PROCEDURE GetAllPsychologists(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist;
END GetAllPsychologists;

END AdminPackage;
/

create role RLClient;
grant create session to RLClient;
grant execute on CLientPackage to RLClient;
grant restricted session to RLClient;
create user ClientUser
IDENTIFIED by Qwerty12345;
grant RLClient to ClientUser;

create or replace package CLientPackage as
PROCEDURE GetClientById( p_Id_Client IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE InsertNewClient(p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Client_Id OUT NUMBER);

PROCEDURE AuthenticateClient(p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_AuthenticationResult OUT NUMBER);

PROCEDURE UpdateClientInfo(p_Id_Client IN NUMBER, p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllClients(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE CreateReview(p_Photo_Review IN BLOB, p_Name_Client IN VARCHAR2, p_Id_Client IN NUMBER, p_Review_Text IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE DeleteReviewById(p_Id IN NUMBER, p_Success OUT NUMBER);

PROCEDURE UpdateReview(p_Review_Id IN NUMBER, p_Photo_Review IN BLOB, p_Name_Client IN VARCHAR2, p_Review_Text IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllReviews(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetTimeTableByPsychologistId(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetSpecializationByName(p_SpecializationName IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetDegreeByName(p_Degree_Name IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetProcedureById(p_Procedure_Id IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllProcedures(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE AddVoucher(p_Date_Voucher IN DATE, p_Time_Voucher_Start IN TIMESTAMP, p_Time_Voucher_End IN TIMESTAMP, p_Id_Psychologist IN NUMBER, p_Ordered IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetVoucherById(p_Id_Voucher IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllVouchers(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE DeleteVoucher(p_Id_Voucher IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteOldVouchers(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteNotOrdered(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteAllById(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE UpdateVoucher(p_Id_Voucher IN NUMBER, p_Date_Voucher IN DATE, p_Time_Voucher_Start IN TIMESTAMP, p_Time_Voucher_End IN TIMESTAMP, p_Id_Psychologist IN NUMBER, p_Ordered IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE CreateBooking(p_Date_Booking IN DATE, p_Id_Procedure IN NUMBER, p_Id_Client IN NUMBER, p_Id_Psychologist IN NUMBER, p_Id_Voucher IN NUMBER, p_Time_Booking IN TIMESTAMP, p_Result OUT NUMBER);

PROCEDURE GetBookingById(p_BookingId IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllBookings(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE DeleteBooking(p_BookingId IN NUMBER, p_Success OUT NUMBER);

PROCEDURE DeleteBookingByIdPsy(p_Id_Psychologist IN NUMBER, p_Success OUT NUMBER);

PROCEDURE GetPsychologistById(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllPsychologists(p_Cursor OUT SYS_REFCURSOR);

END ClientPackage;
/
CREATE OR REPLACE PACKAGE BODY ClientPackage AS

PROCEDURE GetClientById(
    p_Id_Client IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client
        WHERE Id_client = p_Id_Client;
END GetClientById;

PROCEDURE InsertNewClient(
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Client_Id OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientData(p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- ���� ������ ������ ���������, ��������� ������ ������������
    IF v_Valid THEN
        INSERT INTO Client (Name_Client, Surname_Client, Login_Client, Password_Client, Photo_Client, Mail_Client)
        VALUES (p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
        COMMIT; -- �������� ��������� � ���� ������
        SELECT Id_client
        INTO p_Client_Id FROM Client
        WHERE Login_Client = p_Login_Client; 
    ELSE
        p_Client_Id := -1; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Client_Id := -1;
END InsertNewClient;

PROCEDURE AuthenticateClient(
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_AuthenticationResult OUT NUMBER
)
IS
    v_ValidData BOOLEAN := ValidateAuthenticationData(p_Login_Client, p_Password_Client);
    v_Client_Count NUMBER;
    v_Client_id NUMBER;
    pass Client.Password_Client%Type;
BEGIN
    -- �������� ���������� ������
    pass := EncryptPassword(p_Password_Client);
    IF v_ValidData THEN
        SELECT COUNT(*)
        INTO v_Client_Count
        FROM Client
        WHERE Login_Client = p_Login_Client AND Password_Client = pass;
        IF v_Client_Count > 0 THEN
            SELECT Id_client
            INTO v_Client_id
            FROM Client
            WHERE Login_Client = p_Login_Client AND Password_Client = pass;

            p_AuthenticationResult := v_Client_id; -- �������� ��������������
        ELSE
            p_AuthenticationResult := -5; -- ��������� ��������������
        END IF;
    ELSE
        p_AuthenticationResult := -20; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_AuthenticationResult := -1; -- ������ � �������� ����������
END AuthenticateClient;

PROCEDURE UpdateClientInfo(
    p_Id_Client IN NUMBER,
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientUpdateData(p_Name_Client, p_Surname_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � �������
        UPDATE Client
        SET
            Name_Client = p_Name_Client,
            Surname_Client = p_Surname_Client,
            Photo_Client = p_Photo_Client,
            Mail_Client = p_Mail_Client
        WHERE Id_Client = p_Id_Client;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ����������
    ELSE
        p_Result := 0; -- ������ �� ���� ��������� (��������, ������ � ��������� Id_Client �� ������)
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateClientInfo;

PROCEDURE GetAllClients(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client;
END GetAllClients;

PROCEDURE CreateReview(
    p_Photo_Review in BLOB,
    p_Name_Client IN VARCHAR2,
    p_Id_Client IN NUMBER,
    p_Review_Text IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateReviewData(p_Review_Text);
    v_Client_Count NUMBER;
    v_Previous_Review_Count NUMBER;
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- �������� ������������� ������������ �� Id_Client
        SELECT COUNT(*)
        INTO v_Client_Count
        FROM Client
        WHERE Id_Client = p_Id_Client;

        IF v_Client_Count > 0 THEN
            SELECT COUNT(*)
            INTO v_Previous_Review_Count
            FROM Review
            WHERE Id_Client = p_Id_Client;

            IF v_Previous_Review_Count = 0 THEN
                -- ������� ������ � ������� Review
                INSERT INTO Review (Photo_Review, Name_Client, Id_Client, Review)
                VALUES (p_Photo_Review, p_Name_Client, p_Id_Client, p_Review_Text);
    
                COMMIT; -- �������� ��������� � ���� ������
                p_Result := 1; -- �������� �������� ������
            ELSE
                p_Result := 0;
            END IF;
        ELSE
            p_Result := 0; -- ������������ � ��������� Id_Client �� ����������
        END IF;
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END CreateReview;

PROCEDURE DeleteReviewById(
    p_Id IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    DELETE FROM Review
    WHERE Id_Review = p_Id;

    IF SQL%ROWCOUNT > 0 THEN
        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END IF;
END DeleteReviewById;

PROCEDURE UpdateReview(
    p_Review_Id IN NUMBER,
    p_Photo_Review IN BLOB,
    p_Name_Client IN VARCHAR2,
    p_Review_Text IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateReviewData(p_Review_Text);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � ������
        UPDATE Review
        SET 
            Photo_Review = p_Photo_Review,
            Name_Client = p_Name_Client,
            Review = p_Review_Text
        WHERE Id_Review = p_Review_Id;

        -- �������� ���������� ����������� �����
        IF SQL%ROWCOUNT > 0 THEN
            p_Result := 1; -- �������� ����������
            COMMIT; -- �������� ��������� � ���� ������
        ELSE
            p_Result := 0; -- �� ������� �������� ����� (��������, ����� � ��������� Id_Review �� ������)
        END IF;
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateReview;

PROCEDURE GetAllReviews(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Review;
END GetAllReviews;

PROCEDURE GetTimeTableByPsychologistId(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Timetable
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetTimeTableByPsychologistId;

PROCEDURE GetSpecializationByName(
    p_SpecializationName IN VARCHAR2,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Specialization
        WHERE Spezialization_Name = p_SpecializationName;
END GetSpecializationByName;

PROCEDURE GetDegreeByName(
    p_Degree_Name IN VARCHAR2,    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Academic_Degree
        WHERE Academic_Name = p_Degree_Name;
END GetDegreeByName;

PROCEDURE GetProcedureById(
    p_Procedure_Id IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures
        WHERE Id_Procedure = p_Procedure_Id;
END GetProcedureById;

PROCEDURE GetAllProcedures(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures;
END GetAllProcedures;

PROCEDURE AddVoucher(
    p_Date_Voucher IN DATE,
    p_Time_Voucher_Start IN TIMESTAMP,
    p_Time_Voucher_End IN TIMESTAMP,
    p_Id_Psychologist IN NUMBER,
    p_Ordered IN VARCHAR2,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� �������� ���� ordered
    IF UPPER(p_Ordered) IN ('��', '���') THEN
        INSERT INTO Voucher (Date_Voucher,Time_Voucher_Start,Time_Voucher_End,Id_Psychologist,Ordered)
        VALUES (p_Date_Voucher, p_Time_Voucher_Start, p_Time_Voucher_End,p_Id_Psychologist, p_Ordered);
        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� �������
    ELSE
        p_Result := 0; -- �������� �������� ���� ordered
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END AddVoucher;

PROCEDURE GetVoucherById(
    p_Id_Voucher IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Voucher
        WHERE Id_Voucher = p_Id_Voucher;
END GetVoucherById;

PROCEDURE GetAllVouchers(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Voucher;
END GetAllVouchers;

PROCEDURE DeleteVoucher(
    p_Id_Voucher IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    DELETE FROM Voucher
    WHERE Id_Voucher = p_Id_Voucher;

    IF SQL%ROWCOUNT > 0 THEN
        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END IF;
END DeleteVoucher;

PROCEDURE DeleteOldVouchers(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    p_Success := 0; -- ������������� �������� �� ���������

    -- ������ ��� ������, ��������������� ��������
    FOR voucher_rec IN (SELECT *
                        FROM Voucher
                        WHERE Id_Psychologist = p_Id_Psychologist
                          AND Date_Voucher < SYSTIMESTAMP) LOOP

        -- �������� ������� ��� ��������
        IF voucher_rec.Ordered = '���' THEN
            -- �������� ������
            DELETE FROM Voucher
            WHERE Id_Voucher = voucher_rec.Id_Voucher;
        ELSE
            -- �������� ��������� ������������
            FOR booking_rec IN (SELECT *
                                FROM Booking
                                WHERE Id_Psychologist = p_Id_Psychologist
                                  AND Id_Voucher = voucher_rec.Id_Voucher) LOOP

                DELETE FROM Booking
                WHERE Id_Booking = booking_rec.Id_Booking;

            END LOOP;

            -- �������� ������
            DELETE FROM Voucher
            WHERE Id_Voucher = voucher_rec.Id_Voucher;

        END IF;

        p_Success := 1; -- �������� ��������
        COMMIT; -- �������� ��������� � ���� ������
    END LOOP;

END DeleteOldVouchers;

PROCEDURE DeleteNotOrdered(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    -- �������� ������� � Ordered = '���'
    FOR voucher IN (SELECT * FROM Voucher WHERE Ordered = '���' AND Id_Psychologist = p_Id_Psychologist)
    LOOP
        DELETE FROM Voucher WHERE Id_Voucher = voucher.Id_Voucher;
    END LOOP;

    COMMIT; -- �������� ��������� � ���� ������

    p_Success := 1; -- �������� ��������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Success := 0; -- ������ � �������� ����������
END DeleteNotOrdered;

PROCEDURE DeleteAllById(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
IS
BEGIN
    -- �������� ���� ������� �� Id_Psychologist
    FOR voucher IN (SELECT * FROM Voucher WHERE Id_Psychologist = p_Id_Psychologist)
    LOOP
        DELETE FROM Voucher WHERE Id_Voucher = voucher.Id_Voucher;
    END LOOP;

    COMMIT; -- �������� ��������� � ���� ������

    p_Success := 1; -- �������� ��������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Success := 0; -- ������ � �������� ����������
END DeleteAllById;

PROCEDURE UpdateVoucher(
    p_Id_Voucher IN NUMBER,
    p_Date_Voucher IN DATE,
    p_Time_Voucher_Start IN TIMESTAMP,
    p_Time_Voucher_End IN TIMESTAMP,
    p_Id_Psychologist IN NUMBER,
    p_Ordered IN VARCHAR2,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� �������� ���� ordered
    IF UPPER(p_Ordered) IN ('��', '���') THEN
        -- ���������� ���������� � ������
        UPDATE Voucher
        SET
            Date_Voucher = p_Date_Voucher,
            Time_Voucher_Start = p_Time_Voucher_Start,
            Time_Voucher_End = p_Time_Voucher_End,
            Id_Psychologist = p_Id_Psychologist,
            Ordered = p_Ordered
        WHERE Id_Voucher = p_Id_Voucher;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ���������� ������
    ELSE
        p_Result := 0; -- �������� �������� ���� ordered
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateVoucher;

PROCEDURE CreateBooking(
    p_Date_Booking IN DATE,
    p_Id_Procedure IN NUMBER,
    p_Id_Client IN NUMBER,
    p_Id_Psychologist IN NUMBER,
    p_Id_Voucher IN NUMBER,
    p_Time_Booking IN TIMESTAMP,
    p_Result OUT NUMBER
)
AS
BEGIN
    -- �������� ������� ���������� ������� � ��������� Id_Voucher
    DECLARE
        v_Voucher_Count NUMBER;
    BEGIN
        SELECT COUNT(*)
        INTO v_Voucher_Count
        FROM Voucher
        WHERE Id_Voucher = p_Id_Voucher AND Ordered = '���';

        IF v_Voucher_Count = 0 THEN
            -- ������ ����� ��� �� ����������
            p_Result := 0;
            RETURN;
        END IF;
    END;

    -- �������� ������������
    INSERT INTO Booking (
    Date_Booking,
    Id_Procedure,
    Id_Client,
    Id_Psychologist,
    Id_Voucher,
    Time_Booking
    ) VALUES (
    p_Date_Booking,
    p_Id_Procedure,
    p_Id_Client,
    p_Id_Psychologist,
    p_Id_Voucher,
    p_Time_Booking
    );

    COMMIT; -- �������� ��������� � ���� ������
    p_Result := 1; -- �������� �������� ������������
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END CreateBooking;

PROCEDURE GetBookingById(
    p_BookingId IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Booking
        WHERE Id_Booking = p_BookingId;
END GetBookingById;

PROCEDURE GetAllBookings(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Booking;
END GetAllBookings;

PROCEDURE DeleteBooking(
    p_BookingId IN NUMBER,
    p_Success OUT NUMBER
)
AS
BEGIN
    -- ������� �������� ������
    DELETE FROM Booking
    WHERE Id_Booking = p_BookingId;

    IF SQL%ROWCOUNT > 0 THEN
        -- �������� ��������
        COMMIT;
        p_Success := 1;
    ELSE
        -- ������ �� �������
        p_Success := 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBooking;

PROCEDURE DeleteBookingByIdPsy(
    p_Id_Psychologist IN NUMBER,
    p_Success OUT NUMBER
)
AS
BEGIN
    -- ������� �������� �������
    DELETE FROM Booking
    WHERE Id_Psychologist = p_Id_Psychologist;

    IF SQL%ROWCOUNT > 0 THEN
        -- �������� ��������
        COMMIT;
        p_Success := 1;
    ELSE
        -- ������ �� �������
        p_Success := 0;
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBookingByIdPsy;

PROCEDURE GetPsychologistById(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetPsychologistById;

PROCEDURE GetAllPsychologists(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist;
END GetAllPsychologists;
END ClientPackage;
/


create role RLGuest;
grant create session to RLGuest;
grant execute on GuestPackage to RLGuest;
grant restricted session to RLGuest;

create user GuestUser
IDENTIFIED by Qwerty12345;
grant RLGuest to GuestUser;

create or replace package GuestPackage as

PROCEDURE GetClientById(p_Id_Client IN NUMBER,p_Cursor OUT SYS_REFCURSOR);

PROCEDURE InsertNewClient(p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Client_Id OUT NUMBER);

PROCEDURE AuthenticateClient(p_Login_Client IN VARCHAR2, p_Password_Client IN VARCHAR2, p_AuthenticationResult OUT NUMBER);

PROCEDURE UpdateClientInfo(p_Id_Client IN NUMBER, p_Name_Client IN VARCHAR2, p_Surname_Client IN VARCHAR2, p_Photo_Client IN BLOB, p_Mail_Client IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllClients(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE UpdateReview(p_Review_Id IN NUMBER, p_Photo_Review IN BLOB, p_Name_Client IN VARCHAR2, p_Review_Text IN VARCHAR2, p_Result OUT NUMBER);

PROCEDURE GetAllReviews(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetTimeTableByPsychologistId(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetSpecializationByName(p_SpecializationName IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetDegreeByName(p_Degree_Name IN VARCHAR2, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetProcedureById(p_Procedure_Id IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllProcedures(p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetPsychologistById(p_Id_Psychologist IN NUMBER, p_Cursor OUT SYS_REFCURSOR);

PROCEDURE GetAllPsychologists(p_Cursor OUT SYS_REFCURSOR);

END GuestPackage;
/

CREATE OR REPLACE PACKAGE BODY GuestPackage AS

PROCEDURE GetClientById(
    p_Id_Client IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client
        WHERE Id_client = p_Id_Client;
END GetClientById;

PROCEDURE InsertNewClient(
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Client_Id OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientData(p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- ���� ������ ������ ���������, ��������� ������ ������������
    IF v_Valid THEN
        INSERT INTO Client (Name_Client, Surname_Client, Login_Client, Password_Client, Photo_Client, Mail_Client)
        VALUES (p_Name_Client, p_Surname_Client, p_Login_Client, p_Password_Client, p_Photo_Client, p_Mail_Client);
        COMMIT; -- �������� ��������� � ���� ������
        SELECT Id_client
        INTO p_Client_Id FROM Client
        WHERE Login_Client = p_Login_Client; 
    ELSE
        p_Client_Id := -1; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
    p_Client_Id := -2;
    raise;
        
END InsertNewClient;

PROCEDURE AuthenticateClient(
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_AuthenticationResult OUT NUMBER
)
IS
    v_ValidData BOOLEAN := ValidateAuthenticationData(p_Login_Client, p_Password_Client);
    v_Client_Count NUMBER;
    v_Client_id NUMBER;
    pass Client.Password_Client%Type;
BEGIN
    -- �������� ���������� ������
    pass := EncryptPassword(p_Password_Client);
    IF v_ValidData THEN
        SELECT COUNT(*)
        INTO v_Client_Count
        FROM Client
        WHERE Login_Client = p_Login_Client AND Password_Client = pass;
        IF v_Client_Count > 0 THEN
            SELECT Id_client
            INTO v_Client_id
            FROM Client
            WHERE Login_Client = p_Login_Client AND Password_Client = pass;

            p_AuthenticationResult := v_Client_id; -- �������� ��������������
        ELSE
            p_AuthenticationResult := -5; -- ��������� ��������������
        END IF;
    ELSE
        p_AuthenticationResult := -20; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_AuthenticationResult := -1; -- ������ � �������� ����������
END AuthenticateClient;

PROCEDURE UpdateClientInfo(
    p_Id_Client IN NUMBER,
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateClientUpdateData(p_Name_Client, p_Surname_Client, p_Photo_Client, p_Mail_Client);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � �������
        UPDATE Client
        SET
            Name_Client = p_Name_Client,
            Surname_Client = p_Surname_Client,
            Photo_Client = p_Photo_Client,
            Mail_Client = p_Mail_Client
        WHERE Id_Client = p_Id_Client;

        COMMIT; -- �������� ��������� � ���� ������
        p_Result := 1; -- �������� ����������
    ELSE
        p_Result := 0; -- ������ �� ���� ��������� (��������, ������ � ��������� Id_Client �� ������)
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateClientInfo;

PROCEDURE GetAllClients(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client;
END GetAllClients;

PROCEDURE UpdateReview(
    p_Review_Id IN NUMBER,
    p_Photo_Review IN BLOB,
    p_Name_Client IN VARCHAR2,
    p_Review_Text IN VARCHAR2,
    p_Result OUT NUMBER
)
IS
    v_Valid BOOLEAN := ValidateReviewData(p_Review_Text);
BEGIN
    -- �������� ���������� ������
    IF v_Valid THEN
        -- ���������� ���������� � ������
        UPDATE Review
        SET 
            Photo_Review = p_Photo_Review,
            Name_Client = p_Name_Client,
            Review = p_Review_Text
        WHERE Id_Review = p_Review_Id;

        -- �������� ���������� ����������� �����
        IF SQL%ROWCOUNT > 0 THEN
            p_Result := 1; -- �������� ����������
            COMMIT; -- �������� ��������� � ���� ������
        ELSE
            p_Result := 0; -- �� ������� �������� ����� (��������, ����� � ��������� Id_Review �� ������)
        END IF;
    ELSE
        p_Result := 0; -- ���������� ������
    END IF;
EXCEPTION
    WHEN OTHERS THEN
    rollback;
        p_Result := 0; -- ������ � �������� ����������
END UpdateReview;

PROCEDURE GetAllReviews(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Review;
END GetAllReviews;

PROCEDURE GetTimeTableByPsychologistId(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Timetable
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetTimeTableByPsychologistId;

PROCEDURE GetSpecializationByName(
    p_SpecializationName IN VARCHAR2,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Specialization
        WHERE Spezialization_Name = p_SpecializationName;
END GetSpecializationByName;

PROCEDURE GetDegreeByName(
    p_Degree_Name IN VARCHAR2,    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Academic_Degree
        WHERE Academic_Name = p_Degree_Name;
END GetDegreeByName;

PROCEDURE GetProcedureById(
    p_Procedure_Id IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures
        WHERE Id_Procedure = p_Procedure_Id;
END GetProcedureById;

PROCEDURE GetAllProcedures(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures;
END GetAllProcedures;

PROCEDURE GetPsychologistById(
    p_Id_Psychologist IN NUMBER,
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist
        WHERE Id_Psychologist = p_Id_Psychologist;
END GetPsychologistById;

PROCEDURE GetAllPsychologists(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist;
END GetAllPsychologists;

END GuestPackage;
/
