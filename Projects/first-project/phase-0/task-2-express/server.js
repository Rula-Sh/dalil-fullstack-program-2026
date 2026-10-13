const express = require("express");
const cors = require("cors"); // Source - https://stackoverflow.com/a/71498905
const app = express();
const port = 3000;

app.use(cors()); // should be used before listening

// http://localhost:3000/api/hello
app.get("/api/hello", (req, res) => {
  res.json({ message: "Hello World!" });
});

// http://localhost:3000/api/expenses
app.get("/api/expenses", (req, res) => {
  res.send([
    {
      id: 1,
      title: "Lunch",
      amount: 4.5,
      category: "Food",
      date: "2026-01-15",
    },
    {
      id: 2,
      title: "Bus",
      amount: 1.2,
      category: "Transport",
      date: "2026-01-28",
    },
  ]);
});

app.listen(port, () => {
  console.log(`Server is listening on http://localhost:${port}
    try the below URLs:
    - http://localhost:3000/api/hello 
    - http://localhost:3000/api/expenses`);
});
