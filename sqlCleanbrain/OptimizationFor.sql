SELECT *
FROM CustomerInteraction
WHERE ROWNUM <= 2000
  AND Success = 'ÄÀ';
  
  drop index idx_CustomerInteraction_Success;
 CREATE INDEX idx_CustomerInteraction_Success
ON CustomerInteraction(Success); 
  
select count(*) from CustomerInteraction;
DECLARE
    v_Psychologist_Id NUMBER;
    v_Client_Id NUMBER;
    v_Timestamp TIMESTAMP(9);
    v_Success VARCHAR2(10);
BEGIN
    FOR i IN 1..100000 LOOP
        v_Psychologist_Id := 70; 
        v_Client_Id := 153;       
        v_Timestamp := CURRENT_TIMESTAMP + INTERVAL '1' SECOND * i;
        v_Success := CASE WHEN MOD(i, 2) = 0 THEN 'ÄÀ' ELSE 'ÍÅÒ' END;
        INSERT INTO CustomerInteraction(Id_Psychologist, Id_Client, Interaction_Timestamp, Success)
        VALUES (v_Psychologist_Id, v_Client_Id, v_Timestamp, v_Success);
        IF MOD(i, 1000) = 0 THEN
            COMMIT;
        END IF;
    END LOOP;
    COMMIT;
END;
/