CREATE TABLE Employees (
	Emp_ID   INT PRIMARY KEY,
	Emp_Name VARCHAR(50),
	Salary   DECIMAL(8,2)
);

INSERT INTO Employees VALUES (1, 'Ali',  500),
							 (2, 'Sara', 600),
							 (3, 'Omar', 700);

-- Write a single SQL script that performs the following steps in order:
-- 1) Start a transaction.
-- 2) Increase the salary of Ali by 100.
-- 3) Create a savepoint named SP1.
-- 4) Delete the employee Omar.
-- 5) You realize deleting Omar was a mistake — undo only the delete operation, keeping Ali's raise.
-- 6) Save the remaining changes permanently.
-- 7) Display the final content of the table.


-------- Answer: --------
-- 1) Start a transaction.
BEGIN;
-- 2) Increase the salary of Ali by 100.
UPDATE Employees SET Salary = Salary+100 WHERE Emp_Name = 'Ali';
-- 3) Create a savepoint named SP1.
SAVEPOINT SP1;
-- 4) Delete the employee Omar.
DELETE FROM Employees WHERE Emp_Name = 'Omar';
-- 5) undo only the delete operation, keeping Ali's raise.
ROLLBACK TO SP1;
-- 6) Save the remaining changes permanently.
COMMIT;
-- 7) Display the final content of the table.
SELECT * FROM Employees;
