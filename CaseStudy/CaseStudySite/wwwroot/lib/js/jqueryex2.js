$(function () {

    const studentData = JSON.parse(
      `[{ "id": 123, "firstname": "Teachers", "lastname": "Pet" },
        { "id": 234, "firstname": "Brown", "lastname": "Nose" },
        { "id": 345, "firstname": "Always", "lastname": "Late" }]`);

    $("#loadbutton").click(e => {
        let html = "";

        studentData.map(student => {
            html += `<div id="${student.id}" 
                        class="list-group-item">${student.firstname},${student.lastname}</div>`;
        });

        $("#studentList").html(html);
        $("#loadbutton").hide();
    });

    $("#studentList").click(e => {
        const student = studentData.find(s => s.id === parseInt(e.target.id));

        $("#results").text(`you selected ${student.firstname}, ${student.lastname}`);
    });
});