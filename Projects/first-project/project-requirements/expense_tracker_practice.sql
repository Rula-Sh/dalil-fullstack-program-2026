CREATE TABLE students(
	student_id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	name VARCHAR(40)
);

INSERT INTO students (name) VALUES ('user1'),('user2');

SELECT * FROM students;
