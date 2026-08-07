-- ================================================================================================================================================================================
-- =========================================================================== Part A — PL/pgSQL Basics ===========================================================================
-- ================================================================================================================================================================================

--=========================================================== Phase 1: Blocks & Variables ===========================================================

-- 1. Write an anonymous DO block with a full DECLARE / BEGIN / EXCEPTION / END structure that declares a variable using %TYPE against members.email, retrieves the email of the member with the lowest member id using SELECT INTO, and prints it with RAISE NOTICE. 
-- 2. In the same block, declare a second variable (a plain type, e.g. INTEGER) to hold that member's total number of bookings, calculate it with a query, and print both values together. 
DO $$
	DECLARE
		v_email members.email%TYPE;
		v_total_bookings INT;
	BEGIN
		SELECT email INTO v_email FROM members ORDER BY member_id LIMIT 1;

		SELECT COUNT(b.booking_id) INTO v_total_bookings FROM members m JOIN bookings b ON m.member_id = b.member_id WHERE email = v_email;
		
		RAISE NOTICE 'The lowest member id (%) has % bookings.', v_email members, v_total_bookings;
		EXCEPTION
			WHEN OTHERS THEN 
				RAISE NOTICE 'Unexpected Error: %', SQLERRM;
	END;
$$;


-- 3. Write a short note (2–3 sentences) on the difference between running this logic as a plain SQL SELECT versus as a PL/pgSQL block — what procedural capability did you gain? 

/* if this was written in a SELECT statement, it would have been longer, harder to read, and may have more probability to make mistakes when writing it. 
	On the other hand, using a PL/pgSQL block gave more readablility to the code, clearer output using RAISE NOTICE, and allowed for exception handling if errors occurred. */


--========================================================== Phase 2: Control Flow & Loops ==========================================================

-- 4. Write a DO block that checks a specific member's status column with IF / ELSIF / ELSE and RAISE NOTICE a different message for 'active', 'inactive', and any other value. 

DO $$
	DECLARE
		v_member_id INT := 1; -- used member_id 1
		v_status members.status%TYPE;
	BEGIN
		SELECT status INTO v_status FROM members WHERE member_id = v_member_id;
		IF v_status = 'active' OR v_status = 'inactive' THEN
			RAISE NOTICE 'The member account with ID % is %',v_member_id, v_status;
		ELSE
			RAISE NOTICE 'The member account with ID % has an invalid status',v_member_id;
		END IF;
	END;
$$;
-- NOTE: in order to test the last condition (if the member status was neither active or not) we can run the following command:
-- ALTER TABLE members DROP CONSTRAINT members_status_check;
-- ALTER TABLE members ADD CONSTRAINT members_status_check CHECK(status IN('active','inactive')); -- to bring back the constraint after the question

-- if needed
-- SELECT * FROM members where member_id = 1; 
-- UPDATE members SET status = 'inactive'; -- 'active' / 'idle'


-- 5. Write a FOR loop that iterates over all classes belonging to one specific category and prints each class name. 

DO $$
	DECLARE
		v_category_name categories.category_name%TYPE := 'Cardio'; -- 'Yoga', 'Cardio', 'Boxing', 'Zumba', 'Swimming'
		v_class_name classes.class_name%TYPE;
	BEGIN
		RAISE NOTICE 'The classes that belong to % category:', v_category_name;
		FOR v_class_name IN
			SELECT cl.class_name FROM classes cl JOIN categories ca ON cl.category_id = ca.category_id WHERE category_name = v_category_name
		LOOP
			RAISE NOTICE '% class', v_class_name;
		END LOOP;
		IF v_class_name IS NULL THEN
				RAISE NOTICE 'NONE';
		END IF;
	END;
$$;


-- 6. Write a WHILE loop that counts how many sessions a specific trainer has led, incrementing a counter variable until all their sessions have been checked (do not just use COUNT() directly — the point is to practice the loop). 
-- Using CURSOR with WHILE
DO $$
	DECLARE 
		v_trainer_id trainers.trainer_id%TYPE :=2;  -- used trainer_id 2
		v_sessions_count INT :=0;
		
		trainer_cursor CURSOR FOR 
			SELECT t.trainer_id FROM trainers t JOIN sessions s ON t.trainer_id = s.trainer_id WHERE t.trainer_id = v_trainer_id;
		trainer_record RECORD;
	BEGIN
		OPEN trainer_cursor;
			FETCH trainer_cursor INTO trainer_record; -- get the first record
		WHILE FOUND LOOP 
			v_sessions_count = v_sessions_count +1;
			FETCH trainer_cursor INTO trainer_record; -- loop over the other records as long as they exist using the condition FOUND
		END LOOP;
		RAISE NOTICE 'The trainer with ID % has % sessions',v_trainer_id, v_sessions_count;
		CLOSE trainer_cursor;
	END;
$$;

-- Using FOR LOOP
DO $$
	DECLARE 
		trainer_record RECORD;
		v_sessions_count INT :=0;
	BEGIN
		FOR trainer_record IN
			SELECT t.trainer_id FROM trainers t JOIN sessions s ON t.trainer_id = s.trainer_id WHERE t.trainer_id = 2  -- used trainer_id 2
		LOOP
			v_sessions_count = v_sessions_count +1;
		END LOOP;
		RAISE NOTICE 'The trainer with ID % has % sessions',trainer_record.trainer_id, v_sessions_count;
	END;
$$;
-- SELECT t.trainer_id, s.session_id FROM trainers t JOIN sessions s ON t.trainer_id = s.trainer_id WHERE t.trainer_id =2 ; -- if needed


-- =============================================================== Phase 3: Functions ===============================================================

-- Note: For each function, show the CREATE FUNCTION statement and a sample SELECT calling it. 

-- 7. get_member_full_name (RETURNS TEXT) — returns the member's full name (first + last).

-- !!!!!!!!! NOTE: Since its requested to return the full name as (first + last). This will require updating the members table structure. Please find at the bottom of the file "DB UPDATES" section. !!!!!!!!!
CREATE OR REPLACE FUNCTION get_member_full_name(p_member_id members.member_id%TYPE)
RETURNS TEXT AS $$
	DECLARE
		v_first_name members.first_name%TYPE;
		v_last_name  members.last_name%TYPE;
		v_full_name TEXT;
	BEGIN
		SELECT first_name, last_name INTO v_first_name, v_last_name FROM members WHERE member_id = p_member_id;
		v_full_name = v_first_name || ' ' || v_last_name;
		RETURN (v_full_name);
		/* if it was not requested to return the full name as (first + last), i can simply use this in the BEGIN block: 
				RETURN(SELECT full_name v_full_name FROM members WHERE member_id = p_member_id;) 
			without the need to run the commands under "DB UPDATES" section and only declare v_full_name as: 
				v_full_name members.full_name%TYPE; */
	END;
$$ LANGUAGE plpgsql;

SELECT get_member_full_name(1);


-- 8. get_average_rating_by_trainer (RETURNS NUMERIC )— returns the average booking rating across all sessions led by that trainer. 

CREATE OR REPLACE FUNCTION get_average_rating_by_trainer(p_trainer_id trainers.trainer_id%TYPE)
RETURNS NUMERIC(3,2) AS $$
	BEGIN
		RETURN (SELECT ROUND(AVG(b.rating), 2) FROM bookings b  -- used ROUND so that i can get results like 4.50 instead of 4.5000000000000000
			RIGHT JOIN sessions s ON s.session_id = b.session_id
			RIGHT JOIN trainers t ON t.trainer_id = s.trainer_id 
				WHERE t.trainer_id = p_trainer_id 
				GROUP BY t.trainer_id ORDER BY t.trainer_id);
	END
$$ LANGUAGE plpgsql;

SELECT get_average_rating_by_trainer(2);


-- 9. calculate_loyalty_points(p_member_id INT) RETURNS INT — calculates loyalty points as (number of  bookings with a rating 5.  
/* The loyalty points should be calculated based on the number of bookings made by that member with a rating of 5 in the Bookings table.
In other words, your solution should use data from both the Members and Bookings tables. The function should identify the specified member using p_member_id, retrieve their bookings, count only those with a rating equal to 5, and return that count as the member's loyalty points. */

CREATE OR REPLACE FUNCTION calculate_loyalty_points(p_member_id INT) 
RETURNS INT AS $$
	DECLARE
		v_loyalty_points members.loyalty_points%TYPE;
	BEGIN
		SELECT COUNT(b.rating) INTO v_loyalty_points FROM bookings b JOIN members m ON b.member_id = m.member_id WHERE rating = 5 AND m.member_id = p_member_id;
		-- UPDATE members SET loyalty_points = v_loyalty_points; WHERE member_id = p_member_id;-- if requested
		RETURN (v_loyalty_points);
		/* --OR
			RETURN (SELECT loyalty_points FROM members WHERE member_id = p_member_id);
			INSERT INTO members SET loyalty_points = v_loyalty_points; -- if expected from me to add it in the answer */  
	END
$$ LANGUAGE plpgsql;

SELECT calculate_loyalty_points(1);
-- SELECT b.booking_id, b.rating FROM bookings b JOIN members m ON b.member_id = m.member_id WHERE rating = 5 AND m.member_id = 1; -- if needed


--=============================================================== Phase 4: Procedures ===============================================================

-- 10. add_new_booking(p_member_id INT, p_session_id INT) — inserts a new row into bookings for that member/session (rating and comment left NULL, to be filled in later). 

ALTER TABLE bookings ADD CONSTRAINT uq_member_id_session_id UNIQUE (member_id, session_id); -- to restrict from adding duplicated values

CREATE OR REPLACE PROCEDURE add_new_booking(p_member_id INT, p_session_id INT)
LANGUAGE plpgsql AS $$
	BEGIN
		INSERT INTO bookings (member_id, session_id) VALUES (p_member_id, p_session_id);
		RAISE NOTICE 'Added booking successfully';
		EXCEPTION
			WHEN unique_violation THEN RAISE NOTICE 'A booking already exists with the same member and session id';
			WHEN foreign_key_violation THEN RAISE NOTICE 'Either the provided member or session id does not exist';
			WHEN OTHERS THEN RAISE NOTICE 'Unexpected error: %', SQLERRM;
	END;
$$;

CALL add_new_booking(3,1); -- 'Added booking successfully'
CALL add_new_booking(1,1); -- 'A booking already exists with the same member and session id'
CALL add_new_booking(1,55); -- session_id 55 does not exist
CALL add_new_booking(55,1); -- booking_id 55 does not exist


-- 11. update_member_status(p_member_id INT, p_new_status VARCHAR) — updates a member's status column. 
CREATE OR REPLACE PROCEDURE update_member_status(p_member_id INT, p_new_status VARCHAR)
LANGUAGE plpgsql AS $$
	BEGIN
		IF p_new_status NOT IN('active', 'inactive') THEN
			RAISE EXCEPTION 'Member status must be either active or inactive';
		ELSE
			UPDATE members SET status = p_new_status WHERE member_id = p_member_id;
			RAISE NOTICE 'member with id % status became %', p_member_id, p_new_status;
		END IF;		
	END;
$$;

CALL update_member_status(1,'active');
CALL update_member_status(1,'inactive');
CALL update_member_status(1,'idle');


-- =================================================== Phase 5: Error Handling & RECORD Variables ===================================================

-- 12. Write a DO block that uses a RECORD variable inside a FOR loop to iterate through every session of one class, printing the session date, room, and trainer id from the record. 
DO $$
	DECLARE 
		rec_session RECORD;
		v_class_id INT := 2; -- used class_id 2
	BEGIN	
		RAISE NOTICE 'Below is the list of sessions (date, room, and trainer_id) of class %', v_class_id;
		FOR rec_session IN 
			SELECT date, room, trainer_id FROM sessions WHERE class_id = v_class_id
		LOOP
			RAISE NOTICE 'Date: %, room: %, trainer_id: %', rec_session.date, rec_session.room, rec_session.trainer_id;
		END LOOP;
	END;
$$;


-- 13. Extend add_new_booking (or write a new version) so that the INSERT is wrapped in a BEGIN / EXCEPTION block. Deliberately trigger an error on purpose (e.g. an invalid session_id) and show that your EXCEPTION block catches it and prints a friendly message instead of crashing. 

-- Already done in "Phase 4: Procedures (Q: 10.)" i will duplicate it here:
ALTER TABLE bookings ADD CONSTRAINT uq_member_id_session_id UNIQUE (member_id, session_id); -- to restrict from adding duplicated values

CREATE OR REPLACE PROCEDURE add_new_booking(p_member_id INT, p_session_id INT)
LANGUAGE plpgsql AS $$
	BEGIN
		INSERT INTO bookings (member_id, session_id) VALUES (p_member_id, p_session_id);
		RAISE NOTICE 'Added booking successfully';
		EXCEPTION
			WHEN unique_violation THEN RAISE NOTICE 'A booking already exists with the same member and session id';
			WHEN foreign_key_violation THEN RAISE NOTICE 'Either the provided member or session id does not exist';
			WHEN OTHERS THEN RAISE NOTICE 'Unexpected error: %', SQLERRM;
	END;
$$;

CALL add_new_booking(3,3); -- 'Added booking successfully'
CALL add_new_booking(1,1); -- 'A booking already exists with the same member and session id'
CALL add_new_booking(1,55); -- session_id 55 does not exist
CALL add_new_booking(55,1); -- booking_id 55 does not exist


--=================================================== Phase 6: Practical Applications of PL/pgSQL ===================================================

-- 14. User Lookup: get_member_by_email-  (or use OUT parameters) — returns the full member row for a given email, or a clear message if not found. 

CREATE OR REPLACE FUNCTION get_member_by_email(p_email members.email %TYPE)
RETURNS TABLE (
	member_id members.member_id%TYPE,
	first_name members.first_name%TYPE, -- added first and last name based on "Phase 3: Functions (Q: 7.)"
	email members.email%TYPE,
	phone_number members.phone_number%TYPE,
	join_date members.join_date%TYPE,
	status members.status%TYPE,
	country members.country%TYPE,
	city members.city%TYPE,
	street members.street%TYPE,
	loyalty_points members.loyalty_points%TYPE,
	last_name members.last_name%TYPE
) AS $$
	BEGIN
		IF NOT EXISTS ( SELECT * FROM members m WHERE m.email = p_email) THEN
			RAISE EXCEPTION 'could not find the member with the email: %', p_email;
		ELSE 
			RETURN QUERY SELECT * FROM members m WHERE m.email = p_email;
		END IF;
	END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_member_by_email('ahmad.hussein@email.com');
SELECT * FROM get_member_by_email('nada.malek@email.com');


-- 15. Data Validation: is_valid_rating -RETURNS BOOLEAN — returns TRUE only if the rating is between 1 and 5. 

-- currently there is a constraint on ratings, so i need to drop it and add invalid rating in order get all possible results from this function
ALTER TABLE bookings DROP CONSTRAINT bookings_rating_check;
-- ALTER TABLE bookings ADD CONSTRAINT bookings_rating_check CHECK(rating BETWEEN 1 AND 5); -- to add it back
UPDATE bookings SET rating = 0 WHERE booking_id = 2; -- to test the invalid rating

CREATE OR REPLACE FUNCTION is_valid_rating(p_booking_id bookings.booking_id%TYPE)
RETURNS BOOLEAN AS $$
	DECLARE
		v_rating bookings.rating%TYPE;
	BEGIN
		SELECT rating INTO v_rating FROM bookings WHERE booking_id = p_booking_id;
		IF v_rating BETWEEN 1 AND 5 THEN
			RETURN TRUE;
		ELSE 
			RETURN FALSE;
		END IF;
	END;
$$ LANGUAGE plpgsql;

SELECT is_valid_rating(1);
SELECT is_valid_rating(2);


-- 16. Batch Operation: apply_loyalty_bonus() — loops through every member with more than 3 bookings and increases their loyalty level by 1, without allowing the value to exceed 5 

-- since the loyalty points were inserted with values > 5, please run the following command:
UPDATE members SET loyalty_points = 0;

CREATE OR REPLACE PROCEDURE apply_loyalty_bonus()
LANGUAGE plpgsql AS $$
	DECLARE
		rec_member_id_and_points RECORD;
	BEGIN
		FOR rec_member_id_and_points IN
			SELECT m.member_id, m.loyalty_points FROM members m JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id HAVING COUNT(b.booking_id) > 3
		LOOP
			rec_member_id_and_points.loyalty_points = rec_member_id_and_points.loyalty_points + 1;
			UPDATE members SET loyalty_points = rec_member_id_and_points.loyalty_points WHERE  member_id = rec_member_id_and_points.member_id AND rec_member_id_and_points.loyalty_points < 5;
			RAISE NOTICE 'member with id: % mow has % loyalty points',rec_member_id_and_points.member_id, rec_member_id_and_points.loyalty_points;
		END LOOP;
	END;	
$$;
CALL apply_loyalty_bonus();
-- if needed
-- SELECT m.member_id, m.loyalty_points, COUNT(b.booking_id) FROM members m JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id HAVING COUNT(b.booking_id) > 3;
-- SELECT m.member_id, b.booking_id FROM members m JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id, b.booking_id ORDER BY m.member_id;


-- 17. Business Logic: a function that, given a session_id, returns how many available seats remain (assume a fixed room capacity of your choice, e.g. 15, defined as a constant inside the function). 

CREATE OR REPLACE FUNCTION get_available_seats(p_session_id sessions.session_id%TYPE)
RETURNS INT AS $$
	DECLARE
		v_seats INT:=15;
		v_avaialabe_seats INT;
	BEGIN
		SELECT COUNT(b.booking_id) INTO v_avaialabe_seats FROM bookings b RIGHT JOIN sessions s ON b.session_id = s.session_id WHERE s.session_id = p_session_id;
		v_avaialabe_seats = v_seats - v_avaialabe_seats;
		RETURN v_avaialabe_seats;
	END;
$$ LANGUAGE plpgsql;

SELECT get_available_seats(1); -- note that other than the inserted values in the table it was also inserted also in "CALL add_new_booking(3,1);" in "Phase 4: Procedures Q:10."
SELECT get_available_seats(2); 
-- SELECT b.booking_id, b.session_id FROM bookings b RIGHT JOIN sessions s ON b.session_id = s.session_id WHERE s.session_id = 1; -- if needed


-- ================================================================================================================================================================================
-- ========================================================================== Part B — Advanced PL/pgSQL ==========================================================================
-- ================================================================================================================================================================================

--======================================================== Phase 7: Named Exception Handling ========================================================

-- 18. Write a DO block or procedure that attempts to insert a new trainer with an email that already exists, and catches the error specifically using the unique_violation named exception, printing a friendly message instead of letting PostgreSQL raise the raw error. 

DO $$
	BEGIN
		INSERT INTO trainers (full_name, email, phone_number) VALUES ('Mohammad Hussein', 'mohammad.hussein@email.com', '+962791111110');
		RAISE NOTICE 'Trainer added successfully.';
		EXCEPTION
			WHEN unique_violation THEN RAISE NOTICE 'The email or phone number provided is already used by another trainer.'; -- i mentioned 'or phone number' since it was set also set as UNIQUE 
			WHEN OTHERS THEN RAISE NOTICE 'Unexpected error: %', SQLERRM;
	END;
$$;
-- SELECT * FROM trainers; -- if needed


-- 19. Write a second block that attempts to insert a booking with a session_id that does not exist, and catches it specifically using the foreign_key_violation named exception. 

-- Already answered similarly in "Phase 4: Procedures (Q: 10.)"
DO $$
	BEGIN
		INSERT INTO bookings (member_id, session_id) VALUES (1, 5); -- used member_id 1 and session_id 5 for example
		RAISE NOTICE 'Added booking successfully';
		EXCEPTION
			WHEN unique_violation THEN RAISE NOTICE 'A booking already exists with the same member and session id';
			WHEN foreign_key_violation THEN RAISE NOTICE 'Either the provided member or session id does not exist';
			WHEN OTHERS THEN RAISE NOTICE 'Unexpected error: %', SQLERRM;
	END;
$$;


--====================================================== Phase 8: Additional PL/pgSQL Practice ======================================================

-- 20. Write a DO block that declares an INTEGER variable, assigns it the total number of members in the members table using SELECT INTO, and prints the result using RAISE NOTICE 
DO $$
	DECLARE
		v_total_members INT;
	BEGIN
		SELECT COUNT(member_id) INTO v_total_members FROM members;
		RAISE NOTICE 'The total number of members is: %',v_total_members;
	END;
$$;

--================================================= Phase 9: Triggers — Validation & Business Rules =================================================

-- 21. Create a  two trigger:  
	-- A. Validation Trigger: choose ONE (rating 1-5 OR prevent duplicate booking). 
	-- B. Business Rule Trigger: prevent updating rating twice. 

CREATE OR REPLACE FUNCTION is_valid_rating()
RETURNS TRIGGER AS $$
	BEGIN
		IF NEW.rating NOT BETWEEN 1 AND 5 THEN
			RAISE EXCEPTION 'rating must be between 1 and 5';
		END IF; 
		RETURN NEW;
	END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_rating_valid BEFORE UPDATE OR INSERT ON bookings FOR EACH ROW EXECUTE FUNCTION is_valid_rating();
UPDATE bookings SET rating = 0 WHERE booking_id=1;
INSERT INTO bookings (member_id, session_id,rating) VALUES (1, 4, 0);

-- in order to prevent updating rating twice, we need to add a column in the table to check if it was updated or not;
ALTER TABLE bookings ADD COLUMN was_rating_updated BOOLEAN DEFAULT FALSE;

CREATE OR REPLACE FUNCTION update_rating()
RETURNS TRIGGER AS $$
	BEGIN
		IF OLD.was_rating_updated = TRUE THEN -- or i can use: 'OLD.rating IS NOT NULL', but in case case, inserting a rating on the first time will be considered an update
			RAISE EXCEPTION 'Updating the rating is only allowed once';
		END IF;
		NEW.was_rating_updated = TRUE;
		RETURN NEW;
	END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_rating_same BEFORE UPDATE ON bookings FOR EACH ROW EXECUTE FUNCTION update_rating();

UPDATE bookings SET rating = 2 WHERE booking_id=1;
UPDATE bookings SET rating = 3 WHERE booking_id=1;


--============================================================= Phase 10: Audit Logging =============================================================

-- 22-Create the audit_log table and implement a trigger function that logs INSERT, UPDATE, and DELETE operations on the bookings table. 
-- this table was created base on the question request, and it will be used to log the opetaionts INSERT, UPDATE, and DELETE in bookings table.
CREATE TABLE audit_log(
	audit_id SERIAL PRIMARY KEY, 
	operation_type TEXT CHECK(operation_type IN('INSERT', 'UPDATE', 'DELETE')),
	booking_id INT,
	member_id INT,
	session_id  INT,
	old_rating INT, 
	new_rating INT, 
	old_comment TEXT, 
	new_comment TEXT, 
	logged_at TIMESTAMP DEFAULT NOW()
);

CREATE OR REPLACE FUNCTION log_bookings()
RETURNS TRIGGER AS $$
	BEGIN
	-- INSERT
		IF TG_OP = 'INSERT' THEN
			INSERT INTO audit_log (operation_type, booking_id, member_id, session_id, old_rating, new_rating, old_comment, new_comment) VALUES (TG_OP ,NEW.booking_id, NEW.member_id, NEW.session_id, null, NEW.rating, null, NEW.comment);
	-- UPDATE
		ELSIF TG_OP = 'UPDATE' THEN
			IF (OLD.rating IS DISTINCT FROM NEW.rating AND NEW.rating BETWEEN 1 AND 5) 
				OR OLD.comment IS DISTINCT FROM NEW.comment THEN 
					INSERT INTO audit_log (operation_type, booking_id, member_id, session_id, old_rating, new_rating, old_comment, new_comment) VALUES (TG_OP ,OLD.booking_id, OLD.member_id, OLD.session_id, OLD.rating, NEW.rating, OLD.comment, NEW.comment);
			END IF;
	-- DELETE
		ELSIF TG_OP = 'DELETE' THEN
			INSERT INTO audit_log (operation_type, booking_id, member_id, session_id, old_rating, new_rating, old_comment, new_comment) VALUES (TG_OP ,OLD.booking_id, OLD.member_id, OLD.session_id, OLD.rating, null, OLD.comment, null);		
		END IF;
		RETURN NEW;
	END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_bookings AFTER INSERT OR UPDATE OR DELETE ON bookings FOR EACH ROW EXECUTE FUNCTION log_bookings();

INSERT INTO bookings (member_id, session_id) VALUES (5, 1);
INSERT INTO bookings (member_id, session_id,rating,comment) VALUES (5, 2, 5,'GREATE!');
UPDATE bookings SET rating = 4 WHERE booking_id=19;
UPDATE bookings SET rating = 5, comment = 'Awesome!' WHERE booking_id=10;
DELETE FROM bookings WHERE booking_id = 20;
DELETE FROM bookings WHERE booking_id = 10;
SELECT * FROM audit_log;

-- if needed
-- SELECT * FROM bookings;
-- TRUNCATaudit_log RESTART IDENTITY;


-- ==================================================== Phase 11: Business Rules in the Database ====================================================

-- 23- Create an AFTER UPDATE trigger on bookings that automatically increases the corresponding member's loyalty level by 1 whenever a NULL rating is filled in for the first time (i.e. the rating changes from NULL to a real value). The loyalty level must never exceed 5. 

CREATE OR REPLACE FUNCTION auto_inc_rating()
RETURNS TRIGGER AS $$
	BEGIN
		IF OLD.rating IS NULL 
			AND OLD.rating IS DISTINCT FROM NEW.rating 
			AND NEW.rating BETWEEN 1 AND 5 THEN
				UPDATE members SET loyalty_points = loyalty_points + 1 WHERE loyalty_points < 5 AND member_id = OLD.member_id;
		END IF;
		RETURN NEW;
	END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_auto_inc_rating AFTER UPDATE ON bookings FOR EACH ROW EXECUTE FUNCTION auto_inc_rating();

INSERT INTO bookings (member_id, session_id) VALUES (5, 5);
UPDATE bookings SET rating = 4 WHERE booking_id = (SELECT booking_id FROM bookings ORDER BY booking_id DESC LIMIT 1); -- upadate the lastly inserted booking (i used update here to trigger the function since it was requested as "AFTER UPDATE")
SELECT loyalty_points FROM members WHERE member_id = 5; -- to see if the member loyalty_points increased


-- ================================================================================================================================================================================
-- ================================================================================== DB UPDATES ==================================================================================
-- ================================================================================================================================================================================

-- 7. get_member_full_name (RETURNS TEXT) — returns the member's full name (first + last).
ALTER TABLE members RENAME COLUMN full_name TO first_name; 
ALTER TABLE members ALTER COLUMN first_name TYPE VARCHAR(25); 
ALTER TABLE members ADD COLUMN last_name VARCHAR(25); 
UPDATE members SET first_name = 'Ahmad', last_name='Hussein' WHERE member_id = 1;
UPDATE members SET first_name = 'Layla', last_name='Masri' WHERE member_id = 2;
UPDATE members SET first_name = 'Omar', last_name='Al-Khatib' WHERE member_id = 3;
UPDATE members SET first_name = 'Fatima', last_name='Zayyad' WHERE member_id = 4;
UPDATE members SET first_name = 'Yusuf', last_name='Ramahi' WHERE member_id = 5;
UPDATE members SET first_name = 'Noor', last_name='Sharif' WHERE member_id = 6;
UPDATE members SET first_name = 'Khalid', last_name='Qudah' WHERE member_id = 7;
UPDATE members SET first_name = 'Samira', last_name='Hassan' WHERE member_id = 8;
UPDATE members SET first_name = 'Ibrahim', last_name='Ali' WHERE member_id = 9;
UPDATE members SET first_name = 'Rania', last_name='Majali' WHERE member_id = 10;



