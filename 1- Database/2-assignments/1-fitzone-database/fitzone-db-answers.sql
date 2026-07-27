-- ================================================================================================================================================================================
--==================================================================== Phase 1: Schema Diagram & Normalization ====================================================================
-- ================================================================================================================================================================================

/* Q: Assume that initially all the system's data lived in one big Unnormalized Table combining member + session + class + trainer + rating data together. 
 	  Show what this table would look like and what problems it has (data duplication, update anomalies, a repeating/multi-valued ratings column, etc.). */

/* It will cause many issues, such as duplication, difficulty in maintaining the database, fixing invalid input, and deleting duplicate entries 
   (since there is no option to restrict duplicates). It will also be time consuming, costly, and takes more storage. Additionally, there is a risk of introducing more bugs. 
   Overall, it will be very hard to manage due to the insufficient structure. */


-- ================================================================================================================================================================================
--================================================================= Phase 2: Create the Database and Tables (DDL) =================================================================
-- ================================================================================================================================================================================

--================================================================== Create Tables ==================================================================

-- Note: I did not add a relationship between classes and members/tutors, since it was not requested or mentioned in the doc. 

CREATE TABLE members (
	member_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	full_name VARCHAR (50) NOT NULL,
	email VARCHAR (60) NOT NULL UNIQUE,
	phone_number INT NOT NULL UNIQUE,
	join_date TIMESTAMPTZ NOT NULL DEFAULT NOW(),
	status VARCHAR(10) CHECK(status IN('active','inactive')) NOT NULL DEFAULT 'active',
	country VARCHAR(20),
	city VARCHAR(30),
	street VARCHAR(40)	
);

CREATE TABLE trainers (
	trainer_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	full_name VARCHAR (50) NOT NULL,
	email VARCHAR (60) NOT NULL UNIQUE,
	phone_number VARCHAR(20) NOT NULL UNIQUE,
	years_of_experience INT DEFAULT 0,
	country VARCHAR(20),
	city VARCHAR(30),
	street VARCHAR(40),
	senior_mentor_id INT REFERENCES trainers (trainer_id) DEFAULT NULL -- has a relationship with its self (a trainer may have one senior mentor)
);

CREATE TABLE categories (
	category_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	category_name VARCHAR (20) NOT NULL
);

CREATE TABLE classes (
	class_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	class_name VARCHAR (50) NOT NULL UNIQUE,
	price INT CHECK (price >= 0) NOT NULL DEFAULT 0,
	start_date DATE NOT NULL,
	end_date DATE NOT NULL,
	category_id INT REFERENCES categories (category_id) ON DELETE CASCADE -- 1:M relationship (a class has one category / a category holds many classes)
);

CREATE TABLE sessions (
	session_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	session_name VARCHAR (50) NOT NULL UNIQUE,
	room VARCHAR (20) NOT NULL,
	date DATE NOT NULL,
	start_time TIME NOT NULL,
	end_time TIME NOT NULL,
	class_id INT REFERENCES classes (class_id) ON DELETE CASCADE,  -- 1:M relationship (a class can have many sessions / a session has one class)
	trainer_id INT REFERENCES trainers (trainer_id) ON DELETE CASCADE -- 1:M relationship (a trainer can have many sessions / a session has one trainer)
);

-- junction table between members and sessions
CREATE TABLE bookings (
	booking_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	booked_at TIMESTAMPTZ  NOT NULL DEFAULT NOW(),
	rating INT CHECK(rating BETWEEN 1 AND 5),
	comment VARCHAR (200),
	member_id INT REFERENCES members (member_id) ON DELETE CASCADE , -- 1:M relationship (a member can have many bookings / a booking has one member)
	session_id INT REFERENCES sessions (session_id) ON DELETE CASCADE  -- 1:M relationship (a session can have many bookings / a booking has one session)
);


-- ================================================================== Alter Tables ==================================================================

-- 1) Add loyalty_points column in members table (default value of 0)
ALTER TABLE members ADD COLUMN loyalty_points INT DEFAULT 0;

-- 2) Change phone_number type VARCHAR(20)
ALTER TABLE members ALTER COLUMN phone_number TYPE VARCHAR(20);

-- 3) Truncate bookings table and restart identity
-- REQUIREMENT: run TRUNCATE command after inserting data in bookings (as a learning exercise)

-- Please run the insert values in the other tables under Phase 3... WHY? in order to insert data in bookings, data must be inserted in members and sessions table (members and sessions table also classes and trainers values to be inserted before them {and classes table needs categories table values to be inserted first}) 
INSERT INTO bookings (booked_at, rating, comment, member_id, session_id) VALUES 
	('2026-07-21 16:00:00', 4, 'Good cardio and stamina building', 1, 11),
	('2026-08-09 08:30:00', 5, 'Advanced boxing is intense!', 1, 14),
	('2026-08-10 09:00:00', 4, 'Excellent trainer', 1, 14);

TRUNCATE TABLE bookings RESTART IDENTITY;


-- ================================================================================================================================================================================
-- ============================================================================= Phase 3: Insert Data =============================================================================
-- ================================================================================================================================================================================

-- Add 10 members min (one member must have zero bookings)
INSERT INTO members (full_name, email, phone_number, join_date, status, country, city, street, loyalty_points) VALUES 
('Ahmad Hussein', 'ahmad.hussein@email.com', '+962791111111', '2026-01-15', 'active', 'Jordan', 'Amman', 'Queen Rania Street', 100),
('Layla Masri', 'layla.masri@email.com', '+962792222222', '2026-02-20', 'inactive', 'Jordan', 'Amman', 'Hussein Street', 90), -- inactive
('Omar Al-Khatib', 'omar.khatib@email.com', '+962793333333', '2026-03-10', 'active', 'Jordan', 'Amman', 'Abdoun Street', 80),
('Fatima Zayyad', 'fatima.zayyad@email.com', '+962794444444', '2026-04-05', 'active', 'Jordan', 'Amman', 'University Street', 70), -- inactive
('Yusuf Ramahi', 'yusuf.ramahi@email.com', '+962795555555', '2026-05-12', 'active', 'Jordan', 'Zarqa', 'Main Street', 60), -- has 0 bookings
('Noor Sharif', 'noor.sharif@email.com', '+962796666666', '2026-06-18', 'active', 'Jordan', 'Irbid', 'Main Street', 50),
('Khalid Qudah', 'khalid.qudah@email.com', '+962797777777', '2026-07-22', 'active', 'Jordan', 'Aqaba', 'Main Street', 40),
('Samira Hassan', 'samira.hassan@email.com', '+962798888888', '2026-08-30', 'active', 'Palestine', 'Jerusalem', 'Main Street', 100),
('Ibrahim Ali', 'ibrahim.ali@email.com', '+962799999999', '2026-09-14', 'active', 'Syria', 'Damascus', 'Tlaa Al-Ali', 0),
('Rania Majali', 'rania.majali@email.com', '+962791010101', '2026-10-01', 'active', 'Iraq', 'Baghdad', 'Main Street', 0);

-- Add 6 trainers min (with one Mentor min)
INSERT INTO trainers (full_name, email, phone_number, years_of_experience, country, city, street, senior_mentor_id) VALUES 
('Mohammad Hussein', 'mohammad.hussein@email.com', '+962791111110', 9, 'Jordan', 'Amman', 'Queen Rania Street', NULL),
('Hala Masri', 'hala.masri@email.com', '+962792222220', 7, 'Jordan', 'Amman', 'Hussein Street', NULL),
('Sami Zayyad', 'sami.zayyad@email.com', '+962793333330', 5, 'Jordan', 'Amman', 'Abdoun Street', NULL),
('Dana Ramahi', 'dana.ramahi@email.com', '+962794444440', 3, 'Jordan', 'Amman', 'University Street', 1),
('Rami Sharif', 'rami.sharif@email.com', '+962795555550', 1, 'Jordan', 'Amman', 'Mecca Street', 2),
('Mona Qudah', 'mona.qudah@email.com', '+962796666660', 0, 'Jordan', 'Amman', 'Yarmouk Street', 3);

-- Add 5 Categories (one category does not have classes 'Zumba')
INSERT INTO categories (category_name) VALUES ('Yoga'), ('Cardio'), ('Boxing'), ('Zumba'), ('Swimming');

-- Add 10 classes (one category (4) does not have any classes)
INSERT INTO classes (class_name, price, start_date, end_date, category_id) VALUES 
	('Yoga level 1', 100, '2026-07-01', '2026-09-30', 1),
	('Yoga level 2', 150,'2026-08-01', '2026-10-30', 1),
	('Cardio Beginners', 150,'2026-07-01', '2026-12-30', 2),
	('Cardio Intermediate', 200,'2026-07-01', '2026-12-30', 2),
	('Cardio Advanced', 350,'2026-07-01', '2026-12-30', 2),
	('Boxing Beginners', 100,'2026-08-01', '2026-9-30', 3),
	('Boxing Advanced', 300,'2026-08-01', '2026-9-30', 3),
	('Swimming Beginners', 150,'2027-01-01', '2027-01-30', 5),
	('Swimming Intermediate', 200,'2027-01-01', '2027-01-30', 5),
	('Swimming Advanced', 350,'2027-01-01', '2027-01-30', 5);

-- Add 20 sessions min
INSERT INTO sessions (session_name, room, date, start_time, end_time, class_id, trainer_id) VALUES
	-- Yoga => class_id: 1 / 2
	('Yoga AM', 'Room A', '2026-07-05', '08:00', '09:00', 1, 1),
	('Yoga Noon', 'Room B', '2026-07-06', '11:00', '13:00', 1, 4),
	('Yoga PM', 'Room A', '2026-07-10', '12:00', '13:00', 1, 1),	
	('Yoga+ AM', 'Room A', '2026-08-15', '07:30', '08:30', 2, 4),
	('Yoga+ PM', 'Room B', '2026-08-16', '19:00', '20:00', 2, 6),
	-- Cardio => class_id: 3 / 4 / 5
	('Cardio Beginners AM', 'Studio 1', '2026-07-10', '07:00', '08:00', 3, 2),
	('Cardio Beginners PM', 'Studio 2', '2026-07-11', '17:00', '18:00', 3, 5),
	('Cardio Intermediate AM', 'Studio 1', '2026-07-15', '09:00', '10:00', 4, 2),
	('Cardio Intermediate PM', 'Studio 2', '2026-07-16', '18:00', '19:00', 4, 5),
	('Cardio Advanced AM', 'Studio 3', '2026-07-20', '10:00', '11:00', 5, 5),
	('Cardio Advanced PM', 'Studio 3', '2026-07-22', '16:00', '17:00', 5, 5),	
	-- Boxing => class_id: 6 / 7
	('Boxing Beginners 1', 'Ring 1', '2026-08-05', '08:00', '09:30', 6, 3),
	('Boxing Beginners 2', 'Ring 1', '2026-08-08', '18:00', '19:30', 6, 6),
	('Boxing Advanced 1', 'Ring 2', '2026-08-10', '09:00', '10:30', 7, 3),
	('Boxing Advanced 2', 'Ring 2', '2026-08-12', '19:00', '20:30', 7, 6),
	-- Swimming Beginners => class_id: 8 / 9 / 10
	('Swimming Beginners AM', 'Pool 1', '2027-01-02', '08:00', '09:00', 8, 2),
	('Swimming Beginners PM', 'Pool 1', '2027-01-03', '14:00', '15:00', 8, 5),
	('Swimming Intermediate AM', 'Pool 2', '2027-01-05', '09:00', '10:00', 9, 2),
	('Swimming Intermediate PM', 'Pool 2', '2027-01-06', '18:00', '19:00', 9, 5),
	('Swimming Advanced AM', 'Pool 2', '2027-01-10', '07:00', '08:00', 10, 5),
	('Swimming Advanced PM', 'Pool 2', '2027-01-12', '15:00', '16:00', 10, 5);

-- Adding 20 bookings (some members have multiple bookings ////// 15 bookings must have a raitng ////// one member (5) does not have a booking)
INSERT INTO bookings (booked_at, rating, comment, member_id, session_id) VALUES 
	('2026-07-04 10:30:00', 5, 'Excellent session! Great start to the day', 1, 1),
	('2026-07-04 11:00:00', 4, 'Very peaceful and relaxing', 3, 2),
	('2026-07-05 17:30:00', 5, 'Great evening class, perfect wind-down', 2, 2),
	('2026-07-05 18:00:00', 3, 'Good but room was a bit warm', 7, 2),
	('2026-07-09 11:30:00', 4, 'Nice midday session', 4, 2),
	('2026-07-09 12:00:00', 5, 'Perfect for lunch break', 8, 3),
	('2026-07-09 06:30:00', 5, 'Perfect morning workout!', 4, 3),
	('2026-07-09 07:00:00', 4, 'Really gets the heart going', 8, 6),
	('2026-07-10 16:30:00', 5, 'Evening session is great', 1, 7),
	('2026-07-10 17:00:00', 4, 'Good intensity level', 9, 7),
	('2026-07-14 08:30:00', 5, 'Challenging but rewarding', 2, 8),
	('2026-07-14 09:00:00', 4, 'Great workout for intermediate level', 6, 9),
	('2026-07-19 09:30:00', 5, 'Advanced class pushes you to limits!', 3, 10),
	('2026-07-19 10:00:00', 4, 'Tough but good workout', 7, 11),
	('2026-07-21 15:30:00', 5, 'Another great advanced session', 1, 15),
	('2026-08-07 17:30:00', 2, 'Good but very basic', 3, 17),
	('2026-08-07 18:00:00', NULL, NULL, 10, 19),
	('2027-01-01 07:30:00', NULL, NULL, 7, 20),
	('2027-01-09 14:30:00', NULL, NULL, 1, 20),
	('2027-01-09 15:00:00', NULL, NULL, 6, 20);

SELECT * FROM bookings ORDER BY booking_id; -- run the query to proof that TRUNCATE TABLE bookings RESTART IDENTITY; worked


-- ================================================================================================================================================================================
--======================================================================== Phase 5: SQL Fundamentals & DML ========================================================================
-- ================================================================================================================================================================================

-- Show all members (full name and email) ordered by join date ascending, using AS to alias the full name column.
SELECT full_name AS "Full Name", email, join_date FROM members ORDER BY join_date ASC;

-- Show the distinct cities found in the members table.
SELECT DISTINCT city FROM members;

-- Show only the members whose status is 'active'.
SELECT * FROM members WHERE status = 'active';

-- UPDATE a trainer's years of experience, using RETURNING to return the row after the update.
-- ALTER TABLE trainers ADD COLUMN years_of_experience INT DEFAULT 0; -- incase it was expected from me to include it after creating the table
UPDATE trainers SET years_of_experience = 10 WHERE trainer_id = 1;

-- DELETE one dummy booking you inserted specifically for this purpose, using RETURNING to return the deleted row.
INSERT INTO bookings (booked_at, rating, comment, member_id, session_id) VALUES ('2026-09-14 10:00:00', 4, 'Great trainer', 9, 15);
DELETE FROM bookings WHERE booking_id = 21 RETURNING *;


-- ================================================================================================================================================================================
-- =================================================================== Phase 6: Security & Administration (DCL) ===================================================================
-- ================================================================================================================================================================================

-- Create a read-only user: Create a new database user named readonly_user with a secure password. The user should be able to connect to the fitzone_db database and have read-only access to all tables in the public schema.
CREATE USER readonly_user WITH PASSWORD 'User_23passwords';
GRANT SELECT ON ALL TABLES IN SCHEMA PUBLIC TO readonly_user;

-- Create an operations manager user: Create a database user named manager_user with a secure password. Grant the user permission to view, add, and update records in all tables within the public schema.
CREATE USER manager_user WITH PASSWORD 'Manager_23passwords';
GRANT SELECT, INSERT, UPDATE ON ALL TABLES IN SCHEMA PUBLIC TO manager_user;

-- Revoke update permission from manager_user ( ANY Table ).
REVOKE UPDATE ON ALL TABLES IN SCHEMA PUBLIC FROM manager_user;

-- Q: Write a short paragraph explaining this database administrator working in a company. Include realistic scenarios where permissions are granted and later revoked.

/* the database administrator is responsible for protecting sensitive data while ensuring employees can do their jobs with minimal privileges. 
For example, a new employee might be given readonly access at first to see the database content on specific tables. 
A manager for example, can have update permissions to fix invalid data in submissions table. 
The administrator can revoke update access on specific tables from the manager for some valid reason. 
These pivileges help the company strengthen data security policies and reduce the risk of accidental changes. */


-- ================================================================================================================================================================================
-- ================================================================================ Phase 7: Joins ================================================================================
-- ================================================================================================================================================================================

-- show every session with its class name and the responsible trainer's name.
SELECT s.session_id, s.session_name, c.class_name, t.full_name trainer_name FROM sessions s JOIN classes c ON s.class_id = c.class_id JOIN trainers t ON s.trainer_id = t.trainer_id;

-- show all members with their booking count, including members with zero bookings.
SELECT m.full_name AS member_name, COUNT(booking_id) AS number_of_bookings FROM members m LEFT JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id ORDER BY m.member_id;

-- show all sessions even if they have no bookings (same idea as the previous question, opposite direction).
SELECT s.session_id, s.session_name, COUNT(booking_id) AS number_of_bookings FROM bookings b RIGHT JOIN members m ON m.member_id = b.member_id RIGHT JOIN sessions s ON s.session_id = b.session_id GROUP BY s.session_id ORDER BY s.session_id;

-- show all categories together with all classes, including the category that currently has zero classes.
SELECT cat.category_id, cat.category_name, COUNT(cla.class_name) AS number_of_classes FROM categories cat LEFT JOIN classes cla ON cat.category_id = cla.category_id GROUP BY cat.category_id ORDER BY category_id;

-- show each trainer along with the name of their Mentor (if any).
SELECT t.trainer_id, t.full_name AS trainer, t.senior_mentor_id, m.full_name AS senior_mentor FROM trainers t LEFT JOIN trainers m ON t.senior_mentor_id = m.trainer_id ORDER BY t.trainer_id;


-- ================================================================================================================================================================================
-- ================================================================= Phase 7: Aggregation , Grouping & Subqueries =================================================================
-- ================================================================================================================================================================================


-- Count  the number of bookings per member.
SELECT m.full_name, COUNT(booking_id) AS number_of_bookings FROM members m LEFT JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id ORDER BY m.member_id;

-- Average rating per trainer, based on the rating column in bookings.
-- i can use "ROUND(AVG(column), 2)" so that the avg result does not return many zeros ////// trainer_id 3 does not have a raiting bcs sessions 12 and 14 do not have any bookings
SELECT t.full_name, AVG(b.rating) FROM bookings b RIGHT JOIN sessions s ON b.session_id = s.session_id RIGHT JOIN trainers t ON t.trainer_id = s.trainer_id WHERE rating IS NOT NULL GROUP BY t.trainer_id ORDER BY t.trainer_id ;

-- Highest and lowest rating  given for each class.
SELECT c.class_id, c.class_name,  MIN(b.rating) AS min_rating, MAX(b.rating) AS max_rating FROM classes c LEFT JOIN sessions s ON s.class_id = c.class_id LEFT JOIN bookings b ON b.session_id = s.session_id GROUP BY c.class_id ORDER BY class_id;

-- Total loyalty_points across all members, grouped by city.
SELECT city, SUM(loyalty_points) FROM members GROUP BY city;

-- Use GROUP BY with HAVING to show categories that have more than two classes.
SELECT cat.category_id, cat.category_name, COUNT(cla.class_id) AS number_of_classes FROM categories cat LEFT JOIN classes cla ON cat.category_id = cla.category_id GROUP BY cat.category_id HAVING COUNT(cla.class_id) > 2 ORDER BY cat.category_id;

-- Show members who have never made a booking (using NOT IN or NOT EXISTS).
SELECT m.full_name, COUNT(b.booking_id) FROM members m LEFT JOIN bookings b ON m.member_id = b.member_id GROUP BY m.member_id, b.booking_id HAVING b.booking_id IS NULL ORDER BY m.member_id;


-- ================================================================================================================================================================================
--=============================================================================== Phase 8: Indexing ===============================================================================
-- ================================================================================================================================================================================

-- Improve the query performance on the members table by creating an index on the column used to search for members by their email address.
SELECT * FROM members WHERE email= 'rania.majali@email.com'; -- 101ms
CREATE INDEX idx_members_email on members(email);
SELECT * FROM members WHERE email= 'rania.majali@email.com'; -- 75ms

-- Improve the query performance on the bookings table by creating an index on the column used to identify sessions.
SELECT * FROM bookings WHERE session_id= 20; -- 104ms
CREATE INDEX idx_bookings_session on bookings(session_id);
SELECT * FROM bookings WHERE session_id= 20; -- 77ms

-- Then drop one of the indexes as a test
DROP INDEX idx_members_email;




-- ==================================================================
-- ====================== For Testing Purposes ======================
-- ==================================================================
SELECT * FROM bookings ORDER BY booking_id;
SELECT * FROM sessions ORDER BY session_id;
SELECT * FROM classes ORDER BY classe_id;
SELECT * FROM categories ORDER BY category_id;
SELECT * FROM members ORDER BY member_id;
SELECT * FROM trainers ORDER BY trainer_id;

DROP TABLE IF EXISTS bookings CASCADE;
DROP TABLE IF EXISTS sessions CASCADE;
DROP TABLE IF EXISTS classes CASCADE;
DROP TABLE IF EXISTS categories CASCADE;
DROP TABLE IF EXISTS members CASCADE;
DROP TABLE IF EXISTS trainers CASCADE;
