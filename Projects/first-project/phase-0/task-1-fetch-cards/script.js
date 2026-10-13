async function populate() {
  const requestURL = "https://jsonplaceholder.typicode.com/users";
  const request = new Request(requestURL);

  try {
    const response = await fetch(request);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    // i used await twice here because fetch() and json() return promises, the first time is to get the response and the second time it to get the body as json
    const users = await response.json();
    listExpenses(users);
  } catch (error) {
    const errModal = new bootstrap.Modal("#failed-to-fetch-modal");
    errModal.show();

    const errorContent = document.querySelector("pre");
    errorContent.textContent = error;
  }
}

function listExpenses(obj) {
  const main = document.querySelector("main");

  for (const user of obj) {
    const card = document.createElement("div");
    card.classList.add("card");
    const myH2 = document.createElement("h2");
    const myPara1 = document.createElement("p");
    const myPara2 = document.createElement("p");
    const myPara3 = document.createElement("p");

    myH2.textContent = user.name;
    myPara1.textContent = `Email: ${user.email}`;
    myPara2.textContent = `City: ${user.address.city}`;
    myPara3.textContent = `Company: ${user.company.name}:`;

    card.appendChild(myH2);
    card.appendChild(myPara1);
    card.appendChild(myPara2);
    card.appendChild(myPara3);

    main.appendChild(card);
  }
}

populate();
