alter session set "_ORACLE_SCRIPT" = true;

create role Guest;
select * from Review;



CREATE OR REPLACE FUNCTION EncryptPassword(p_Password IN VARCHAR2)
RETURN NVARCHAR2
AS
BEGIN
IF p_Password IS NULL
THEN
RETURN NULL;
ELSE
RETURN dbms_crypto.hash(utl_raw.cast_to_raw(TRIM(p_Password)), dbms_crypto.hash_sh256);
END IF;
END;


CREATE OR REPLACE TRIGGER HashPasswordTrigger
BEFORE INSERT ON Client
FOR EACH ROW
BEGIN
    :NEW.Password_Client := EncryptPassword(:NEW.Password_Client);
END;
/

CREATE OR REPLACE FUNCTION ValidateClientData(
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Email_Regex VARCHAR2(100) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$';
    v_Login_Regex VARCHAR2(100) := '^[a-zA-Z0-9]{1,12}$';
    v_Name_Regex VARCHAR2(100) := '^[a-zA-Z�-��-�]{1,15}$';
    v_Surname_Regex VARCHAR2(100) := '^[a-zA-Z�-��-�]{1,15}$';
    v_Login_Count NUMBER;
BEGIN
    -- ��������� ������
    IF LENGTH(p_Name_Client) = 0 OR LENGTH(p_Surname_Client) = 0 OR
       LENGTH(p_Login_Client) = 0 OR LENGTH(p_Password_Client) = 0 OR
       p_Photo_Client IS NULL OR
       LENGTH(p_Mail_Client) = 0 OR
       NOT REGEXP_LIKE(p_Mail_Client, v_Email_Regex) OR
       NOT REGEXP_LIKE(p_Login_Client, v_Login_Regex) OR
       NOT REGEXP_LIKE(p_Name_Client, v_Name_Regex) OR
       NOT REGEXP_LIKE(p_Surname_Client, v_Surname_Regex) THEN
        RETURN false; -- ���������� ������
    ELSE
        SELECT COUNT(*)
            INTO v_Login_Count
            FROM Client
            WHERE Login_Client = p_Login_Client;
    
            IF v_Login_Count > 0 THEN
                RETURN false; -- ����� ��� ������������
            END IF;
        RETURN true; -- ������ �������
    END IF;
END ValidateClientData;
/

CREATE OR REPLACE PROCEDURE GetClientById(
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
/

CREATE OR REPLACE PROCEDURE InsertNewClient(
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
        p_Client_Id := -1;
END InsertNewClient;
/



CREATE OR REPLACE FUNCTION ValidateAuthenticationData(
    p_Login_Client IN VARCHAR2,
    p_Password_Client IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Login_Regex VARCHAR2(100) := '^[a-zA-Z0-9]{1,12}$';
BEGIN
    -- �������� ���������� ������ � ������
    IF NOT REGEXP_LIKE(p_Login_Client, v_Login_Regex) THEN
        RETURN FALSE; -- ���������� ������
    ELSE
        RETURN TRUE; -- ������ �������
    END IF;
END ValidateAuthenticationData;
/


CREATE OR REPLACE PROCEDURE AuthenticateClient(
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
/


-- ��������� �����

CREATE OR REPLACE FUNCTION ValidateClientUpdateData(
    p_Name_Client IN VARCHAR2,
    p_Surname_Client IN VARCHAR2,
    p_Photo_Client IN BLOB,
    p_Mail_Client IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Email_Regex VARCHAR2(100) := '^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$';
    v_Name_Regex VARCHAR2(100) := '^[a-zA-Z�-��-�]{1,15}$';
    v_Surname_Regex VARCHAR2(100) := '^[a-zA-Z�-��-�]{1,15}$';
BEGIN
    -- ��������� ������
    IF LENGTH(p_Name_Client) = 0 OR LENGTH(p_Surname_Client) = 0 OR
       p_Photo_Client IS NULL OR
       LENGTH(p_Mail_Client) = 0 OR
       NOT REGEXP_LIKE(p_Mail_Client, v_Email_Regex) OR
       NOT REGEXP_LIKE(p_Name_Client, v_Name_Regex) OR
       NOT REGEXP_LIKE(p_Surname_Client, v_Surname_Regex) THEN
        RETURN false; -- ���������� ������
    ELSE 
        RETURN true; -- ������ �������
    END IF;
END ValidateClientUpdateData;
/

CREATE OR REPLACE PROCEDURE UpdateClientInfo(
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
        p_Result := 0; -- ������ � �������� ����������
END UpdateClientInfo;
/

CREATE OR REPLACE PROCEDURE GetAllClients(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Client;
END GetAllClients;
/

--Review
CREATE OR REPLACE FUNCTION ValidateReviewData(
    p_Review_Text IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Review_Regex VARCHAR2(500) := '^[a-zA-Z�-��-�0-9.,;:''"![:space:]]{1,130}$'; -- ���������� ��������� ��� ������ ������
BEGIN
    IF NOT REGEXP_LIKE(p_Review_Text, v_Review_Regex) THEN
        RETURN false; -- ���������� ������
    ELSE
        RETURN true; -- ������ �������
    END IF;
END ValidateReviewData;
/

CREATE OR REPLACE PROCEDURE CreateReview(
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
        p_Result := 0; -- ������ � �������� ����������
END CreateReview;
/

CREATE OR REPLACE PROCEDURE DeleteReviewById(
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
/
CREATE OR REPLACE PROCEDURE UpdateReview(
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
        p_Result := 0; -- ������ � �������� ����������
END UpdateReview;
/


CREATE OR REPLACE PROCEDURE GetAllReviews(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Review;
END GetAllReviews;
/

-- Timetable

CREATE OR REPLACE PROCEDURE CreateTimeTable(
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
        p_Result := 0; -- ������ � �������� ����������
END CreateTimeTable;
/

CREATE OR REPLACE PROCEDURE GetTimeTableByPsychologistId(
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
/

CREATE OR REPLACE PROCEDURE UpdateTimeTable(
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
        p_Result := 0; -- ������ � �������� ����������
END UpdateTimeTable;
/

CREATE OR REPLACE PROCEDURE DeleteTimeTableById(
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
        p_Result := 0; -- ������ � �������� ����������
END DeleteTimeTableById;

--Spec
CREATE OR REPLACE FUNCTION ValidateSpecializationData(
    p_Spezialization_Name IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Name_Regex VARCHAR2(100) := '^[a-zA-Z�-��-� ]{1,40}$';
BEGIN
    -- �������� ���������� �������� �������������
    IF NOT REGEXP_LIKE(p_Spezialization_Name, v_Name_Regex) THEN
        RETURN false; -- ���������� ������
    ELSE
        RETURN true; -- ������ �������
    END IF;
END ValidateSpecializationData;
/

CREATE OR REPLACE PROCEDURE AddSpecialization(
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
            p_Result := 0; -- ������ � �������� ����������
END AddSpecialization;
/

CREATE OR REPLACE PROCEDURE GetSpecializationByName(
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
/

--degree

CREATE OR REPLACE FUNCTION ValidateDegreeData(
    p_Degree_Name IN VARCHAR2
) RETURN BOOLEAN
IS
    v_Name_Regex VARCHAR2(100) := '^[a-zA-Z�-��-� ]{1,40}$';
BEGIN
    -- �������� ���������� �������� �������
    IF NOT REGEXP_LIKE(p_Degree_Name, v_Name_Regex) THEN
        RETURN false; -- ���������� ������
    ELSE
        RETURN true; -- ������ �������
    END IF;
END ValidateDegreeData;
/

CREATE OR REPLACE PROCEDURE AddDegree(
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
            p_Result := 0; -- ������ � �������� ����������
END AddDegree;
/

CREATE OR REPLACE PROCEDURE GetDegreeByName(
    p_Degree_Name IN VARCHAR2,    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Academic_Degree
        WHERE Academic_Name = p_Degree_Name;
END GetDegreeByName;
/

--proc

CREATE OR REPLACE FUNCTION IsValidInput(
    p_Name NVARCHAR2,
    p_Price FLOAT,
    p_Depiction NVARCHAR2
) RETURN BOOLEAN
AS
BEGIN
    -- �������� �����
    IF NOT REGEXP_LIKE(p_Name, '^[a-zA-Z�-��-߸�[:space:]]{1,20}$') OR
    NOT REGEXP_LIKE(TO_CHAR(p_Price), '^(10000|[1-9]\d{0,3})$') OR
    NOT REGEXP_LIKE(p_Depiction, '[a-zA-Z�-��-�0-9.,;:''"![:space:]]{1,130}$') THEN
        return false;
    ELSE
        return true;
    END IF;
END IsValidInput;
/

CREATE OR REPLACE PROCEDURE AddProcedure(
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
            p_Result := 0;  -- � ������ ������ ������������� p_Result � 0
END AddProcedure;
/

CREATE OR REPLACE PROCEDURE DeleteProcedure(
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
        p_Result := 0; -- ������ � �������� ����������
END DeleteProcedure;
/


CREATE OR REPLACE PROCEDURE UpdateProcedure(
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
        p_Result := 0;  -- � ������ ������ ������������� p_Result � 0
END UpdateProcedure;
/

CREATE OR REPLACE PROCEDURE GetProcedureById(
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
/

CREATE OR REPLACE PROCEDURE GetAllProcedures(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Procedures;
END GetAllProcedures;
/
--voucher

CREATE OR REPLACE PROCEDURE AddVoucher(
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
        p_Result := 0; -- ������ � �������� ����������
END AddVoucher;
/

CREATE OR REPLACE PROCEDURE GetVoucherById(
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
/

CREATE OR REPLACE PROCEDURE GetAllVouchers(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Voucher;
END GetAllVouchers;
/

CREATE OR REPLACE PROCEDURE DeleteVoucher(
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
/
CREATE OR REPLACE PROCEDURE DeleteOldVouchers(
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
/

CREATE OR REPLACE PROCEDURE DeleteNotOrdered(
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
        p_Success := 0; -- ������ � �������� ����������
END DeleteNotOrdered;
/

CREATE OR REPLACE PROCEDURE DeleteAllById(
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
        p_Success := 0; -- ������ � �������� ����������
END DeleteAllById;
/

CREATE OR REPLACE PROCEDURE UpdateVoucher(
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
        p_Result := 0; -- ������ � �������� ����������
END UpdateVoucher;
/
--booking


CREATE OR REPLACE PROCEDURE CreateBooking(
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
        p_Result := 0; -- ������ � �������� ����������
END CreateBooking;
/

CREATE OR REPLACE PROCEDURE GetBookingById(
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
/

CREATE OR REPLACE PROCEDURE GetAllBookings(
    p_Cursor OUT SYS_REFCURSOR
)
IS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Booking;
END GetAllBookings;
/

CREATE OR REPLACE PROCEDURE DeleteBooking(
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
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBooking;
/

CREATE OR REPLACE PROCEDURE DeleteBookingByIdPsy(
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
        -- ������ � �������� ����������
        p_Success := 0;
END DeleteBookingByIdPsy;
/

--psy
CREATE OR REPLACE FUNCTION ValidatePsychologistData(
    p_Name_Psychologist IN NVARCHAR2,
    p_Surname_Psychologist IN NVARCHAR2,
    p_Patronymic_Psychologist IN NVARCHAR2,
    p_Description IN NVARCHAR2
) RETURN BOOLEAN
IS
    v_Name_Regex VARCHAR2(100) := '^[a-zA-Z�-��-�]{1,15}$';
    v_Description_Regex VARCHAR2(300) := '^[a-zA-Z�-��-�0-9.,;:''"![:space:]]{1,250}$';
BEGIN
    -- �������� ���������� ������
    IF NOT REGEXP_LIKE(p_Name_Psychologist, v_Name_Regex) OR
       NOT REGEXP_LIKE(p_Surname_Psychologist, v_Name_Regex) OR
       NOT REGEXP_LIKE(p_Patronymic_Psychologist, v_Name_Regex) OR
       NOT REGEXP_LIKE(p_Description, v_Description_Regex) THEN
        RETURN FALSE; -- ���������� ������
    ELSE
        RETURN TRUE; -- ������ �������
    END IF;
END ValidatePsychologistData;
/


CREATE OR REPLACE PROCEDURE AddPsychologist(
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
        -- ������ � �������� ����������
        p_Result := 0;
END AddPsychologist;
/

CREATE OR REPLACE PROCEDURE UpdatePsychologist(
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
        p_Result := 0; -- ������ � �������� ����������
END UpdatePsychologist;
/

CREATE OR REPLACE PROCEDURE DeletePsychologist(
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
/

CREATE OR REPLACE PROCEDURE GetPsychologistById(
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
/

CREATE OR REPLACE PROCEDURE GetAllPsychologists(
    p_Cursor OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_Cursor FOR
        SELECT *
        FROM Psychologist;
END GetAllPsychologists;
/
