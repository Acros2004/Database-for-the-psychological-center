CREATE USER CleanBrainGuest IDENTIFIED BY 1122;
select * from Client;
create role Guest;
GRANT EXECUTE ON InsertNewClient TO Guest;
GRANT EXECUTE ON ValidateClientData TO Guest;
GRANT EXECUTE ON AuthenticateClient TO Guest;
GRANT EXECUTE ON ValidateAuthenticationData TO Guest;

GRANT Guest TO CleanBrainGuest;
GRANT RESTRICTED SESSION TO CleanBrainGuest;

select * from Specialization;

-- �������� ����������� ������� Client
DELETE FROM Client;

-- �������� ����������� ������� Specialization
DELETE FROM Specialization;

-- �������� ����������� ������� Academic_Degree
DELETE FROM Academic_Degree;

-- �������� ����������� ������� Psychologist
DELETE FROM Psychologist;

-- �������� ����������� ������� Voucher
DELETE FROM Voucher;

-- �������� ����������� ������� Procedures
DELETE FROM Procedures;

-- �������� ����������� ������� Timetable
DELETE FROM Timetable;

-- �������� ����������� ������� Booking
DELETE FROM Booking;

-- �������� ����������� ������� Review
DELETE FROM Review;

delete from Academic_Degree;
INSERT INTO Academic_Degree (Academic_Name) VALUES ('��������');
INSERT INTO Academic_Degree (Academic_Name) VALUES ('�������');
insert INTO Specialization (Spezialization_Name) VALUES ('����������� ����������');
insert INTO Specialization (Spezialization_Name) VALUES ('���������� ����������');
insert INTO Specialization (Spezialization_Name) VALUES ('����������� ����������');
insert INTO Specialization (Spezialization_Name) VALUES ('����������� ����������');


DECLARE
    v_Result NUMBER;
BEGIN
    -- ����� ��������� AddPsychologist � ��������� �������
    SYSTEM.AdminPackage.AddPsychologist(
        '����', '������', '���������',
        5, EMPTY_BLOB(), '������� ��������',
        '����������� ����������', '�������',
        v_Result
    );

    -- �������� ����������
    IF v_Result = 1 THEN
        DBMS_OUTPUT.PUT_LINE('�������� ������� ��������.');
    ELSE
        DBMS_OUTPUT.PUT_LINE('������ ��� ���������� ���������.');
    END IF;
END;
/
commit;

DECLARE
    v_Client_Id NUMBER;
BEGIN
    SYSTEM.GuestPackage.InsertNewClient(
        p_Name_Client => 'John',
        p_Surname_Client => 'Doe',
        p_Login_Client => 'test123456',
        p_Password_Client => '1122',
        p_Photo_Client => EMPTY_BLOB(), 
        p_Mail_Client => 'nikitakarebo@gmail.com',
        p_Client_Id => v_Client_Id
    );
    IF v_Client_Id > 0 THEN
        DBMS_OUTPUT.PUT_LINE('Test Passed: Client inserted successfully with ID OK ' ||v_Client_Id);
    ELSE
        DBMS_OUTPUT.PUT_LINE('Test Failed: Client insertion failed. Returned ID: NOT OK '|| v_Client_Id);
    END IF;
END;
/

DECLARE
    v_Result NUMBER;
BEGIN
    SYSTEM.GuestPackage.UpdateClientInfo(
        p_Id_Client => 162,
        p_Name_Client => 'Johndsd',
        p_Surname_Client => 'Doedasddas',
        p_Photo_Client => EMPTY_BLOB(), 
        p_Mail_Client => 'nikitakarebo@gmail.com',
        p_Result => v_Result
    );
    IF v_Result = 1 THEN
        DBMS_OUTPUT.PUT_LINE('Test Passed: Client updated successfully with OK ');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Test Failed: Client updation failed NOT OK ');
    END IF;
END;
/

DECLARE
    v_Result NUMBER;
BEGIN
    SYSTEM.GuestPackage.UpdateClientInfo(
        p_Id_Client => 162,
        p_Name_Client => 'Johndsd',
        p_Surname_Client => 'Doedasddas',
        p_Photo_Client => EMPTY_BLOB(), 
        p_Mail_Client => 'nikitakarebo@gmail.com',
        p_Result => v_Result
    );
    IF v_Result = 1 THEN
        DBMS_OUTPUT.PUT_LINE('Test Passed: Client updated successfully with OK ');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Test Failed: Client updation failed NOT OK ');
    END IF;
END;
/

DECLARE
    v_Id_Client NUMBER := 162;
    v_Cursor SYS_REFCURSOR;

    v_Name_Client NVARCHAR2(20);
    v_Surname_Client NVARCHAR2(20);
    v_Login_Client NVARCHAR2(20);
    v_Password_Client NVARCHAR2(99);
    v_Photo_Client BLOB;
    v_Mail_Client NVARCHAR2(30);
BEGIN
    SYSTEM.GuestPackage.GetClientById(p_Id_Client => v_Id_Client, p_Cursor => v_Cursor);
    FETCH v_Cursor INTO
        v_Id_Client,
        v_Name_Client,
        v_Surname_Client,
        v_Login_Client,
        v_Password_Client,
        v_Photo_Client,
        v_Mail_Client;
    DBMS_OUTPUT.PUT_LINE('Id_Client: ' || v_Id_Client);
    DBMS_OUTPUT.PUT_LINE('Name_Client: ' || v_Name_Client);
    DBMS_OUTPUT.PUT_LINE('Surname_Client: ' || v_Surname_Client);
    DBMS_OUTPUT.PUT_LINE('Login_Client: ' || v_Login_Client);
    DBMS_OUTPUT.PUT_LINE('Password_Client: ' || v_Password_Client);
    DBMS_OUTPUT.PUT_LINE('Mail_Client: ' || v_Mail_Client);
    CLOSE v_Cursor;
END;
/

