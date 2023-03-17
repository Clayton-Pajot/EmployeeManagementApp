$(function () {

    const stringData =
        `[{ "id": 123, "firstname": "Teachers", "lastname": "Pet" },
        { "id": 234, "firstname": "Brown", "lastname": "Nose" },
        { "id": 345, "firstname": "Always", "lastname": "Late" }]`;

    sessionStorage.getItem("studentData") === null ? sessionStorage.setItem("studentData", stringData) : null;

    let data = JSON.parse(sessionStorage.getItem("studentData"));

    $("#loadbutton").click(e => {
        let html = "";

        data.map(student => {
            html += `<div id="${student.id}" 
                        class="list-group-item">${student.firstname},${student.lastname}</div>`;
        });

        $("#studentList").html(html);
        $("#loadbutton").hide();
        $("#addbutton").show();
        $("#removebutton").show();
    });

    $("#studentList").click(e => {
        const student = data.find(s => s.id === parseInt(e.target.id));

        $("#results").text(`you selected ${student.firstname}, ${student.lastname}`);
    });

    $("#addbutton").click(e => {
        if (data.length > 0) {
            const student = data[data.length - 1];
            data.push({ "id": student.id + 101, "firstname": "New", "lastname": "Student" });
            $("#results").text(`adding student ${student.id + 101}`);
        }
        else {
            data.push({ "id": 101, "firstname": "new", "lastname": "student" });
        }

        sessionStorage.setItem("studentData", JSON.stringify(data));
        let html = "";

        data.map(student => {
            html += `<div id="${student.id}" 
                        class="list-group-item">${student.firstname},${student.lastname}</div>`;
        });

        $("#studentList").html(html);
    });

    $("#removebutton").click(e => {
        if (data.length > 0) {
            const student = data[data.length - 1];
            data.splice(-1, 1);
            $("#results").text(`removed student ${student.id}`);

            sessionStorage.setItem("studentData", JSON.stringify(data));
            let html = "";
            data.map(student => {
                html += `<div id="${student.id}" 
                        class="list-group-item">${student.firstname},${student.lastname}</div>`;
            });
            $("#studentList").html(html);
        }
        else {
            $("#results").text(`no students to remove`);
        }
    })
});