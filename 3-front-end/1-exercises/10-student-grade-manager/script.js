const students = [];
students.push(
  {
    name: "Sara",
    score: 80,
  },
  {
    name: "Ahmad",
    score: 90,
  },
);

function getGrade(score) {
  if (score >= 90) return "Excellent";
  else if (score >= 75) return "Good";
  else return "Needs Improvement";
}

const nameInput = document.getElementById("nameInput");
const scoreInput = document.getElementById("scoreInput");
const addBtn = document.getElementById("addBtn");
const tbody = document.getElementById("tbody");

const nameError = document.getElementById("nameError");
const scoreError = document.getElementById("scoreError");
const search = document.getElementById("search");
const tableEmpty = document.getElementById("tableEmpty");

document.querySelector("input").addEventListener("keydown", function (event) {
  if (/[0-9]/.test(event.key)) {
    event.preventDefault();
  }
});

search.addEventListener("keyup", () => {
  const searchInput = search.value.toLowerCase();
  const filteredstudents = students.filter((s) =>
    s.name.toLowerCase().startsWith(searchInput),
  );
  render(filteredstudents);
});

addBtn.addEventListener("click", () => {
  const name = nameInput.value;
  const score = scoreInput.value;
  let isValid = true;
  if (name.length == 0) {
    nameError.innerText = "You must insert a student name";
    isValid = false;
  } else if (name.trim().length > 20) {
    nameError.innerText = "Name should not exceed 20 characters";
    isValid = false;
  }
  if (score.length == 0 || score < 0 || score > 100) {
    scoreError.innerText = "Score must be between 0 and 100";
    isValid = false;
  }

  if (isValid) {
    students.push({
      name: name,
      score: score,
    });
    tableEmpty.style.display = "none";
    nameError.innerText = "";
    scoreError.innerText = "";
    search.value = "";
    nameInput.value = "";
    scoreInput.value = "";
    render(students);
  }
});

function render(students) {
  tbody.innerHTML = "";
  for (let i = 0; i < students.length; i++) {
    const std = students[i];
    const tr = document.createElement("tr");
    const tdName = document.createElement("td");
    tdName.textContent = std.name;

    const tdScore = document.createElement("td");
    tdScore.textContent = std.score;

    const tdGrade = document.createElement("td");
    // tdGrade.style.padding = "0";
    const label = document.createElement("div");
    label.className = "label";
    const gradeText = document.createElement("p");
    gradeText.className = "gradeLabel";
    gradeText.textContent = getGrade(std.score);

    label.appendChild(gradeText);
    tdGrade.appendChild(label);

    switch (getGrade(std.score)) {
      case "Excellent":
        gradeText.classList.add("green");
        break;
      case "Good":
        gradeText.classList.add("orange");
        break;
      case "Needs Improvement":
        gradeText.classList.add("red");
        break;
    }
    const tdRemove = document.createElement("td");

    const removeBtn = document.createElement("button");
    removeBtn.className = "removeBtn";
    removeBtn.textContent = "Remove";
    removeBtn.addEventListener("click", () => removeStudent(std)); // ← object passed directly
    tdRemove.appendChild(removeBtn);

    tr.appendChild(tdName);
    tr.appendChild(tdScore);
    tr.appendChild(tdGrade);
    tr.appendChild(tdRemove);
    tbody.appendChild(tr);
  }
}

render(students);

function removeStudent(student) {
  const index = students.indexOf(student);
  students.splice(index, 1);

  if (students.length == 0) {
    tableEmpty.style.display = "block";
  }

  search.value = "";
  render(students);
}

const clearBtn = document.getElementById("clearBtn");

clearBtn.addEventListener("click", function () {
  if (students.length == 0) {
    alert("The students list is already empty");
  } else {
    students.length = 0;
    search.value = "";
    nameInput.value = "";
    scoreInput.value = "";
    render(students);

    tableEmpty.style.display = "block";
  }
});

/*
    // tdRemove.innerHTML = `<button class='removeBtn'>Remove</button>`;
    // tdRemove.innerHTML = "<button id='removeBtn'>Remove</button>";




// const removeBtn = document.getElementById("removeBtn");

// removeBtn.addEventListener("click", function () {
//   let studntName =
//     removeBtn.parentElement.previousSibling.previousSibling.previousSibling
//       .innerText;

//   console.log(studntName);
//   for (let i = 0; i < students.length; i++) {
//     console.log(students[i].name);
//     console.log(students[i].name == studntName);

//     if (students[i].name == studntName) {
//       students.splice(i, 1);
//       console.log(students);
//       render();
//     }
//   }
// });

// const removeBtns = document.querySelectorAll(".removeBtn");

// removeBtns.forEach((button) => {
//   button.addEventListener("click", (event) => {
//     const row = event.target.closest("tr");
//     if (row) {
//       row.remove();
//     }

//     let studntName =
//       button.parentElement.previousSibling.previousSibling.previousSibling
//         .innerText;

//     console.log(studntName);
//     for (let i = 0; i < students.length; i++) {
//       console.log(students[i].name);
//       console.log(students[i].name == studntName);

//       if (students[i].name == studntName) {
//         students.splice(i, 1);
//         console.log(students);
//         render();
//       }
//     }
//   });
// }); */
