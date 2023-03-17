$(function () {
  const stringData =
    `[{ "id": 123, "firstname": "Teachers", "lastname": "Pet" },
      { "id": 234, "firstname": "Brown", "lastname": "Nose" },
      { "id": 345, "firstname": "Always", "lastname": "Late" }]`;

  $("#loadbutton").click(e => {
    const studentData = JSON.parse(stringData);

    let html = "";

    studentData.map(student => {
      html += `<div class="list-group-item">${student.id},${student.firstname},${student.lastname}</div>`;
    });

    $("#studentList").html(html);
    $("#loadbutton").hide();
  });
});