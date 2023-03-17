

$(function () {

    //VALIDATION START
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass();
        if ($("#EmployeeModalForm").valid()) {
            $("#modalstatus").attr("class", "badge bg-success");
            $("#modalstatus").text("data entered is valid");
        }
        else {
            $("#modalstatus").attr("class", "badge bg-danger");
            $("#modalstatus").text("fix errors");
        }
    });

    

    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstName: { maxlength: 25, required: true },
            TextBoxLastName: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstName: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastName: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
        }
    });

    $.validator.addMethod("validTitle", (value) => {
        return (value === "Mr." || value === "Mrs." || value === "Ms." || value === "Dr.");
    }, "");


    $("#getbutton").mouseup(async (e) => {
                try {
                    $("#TextBoxLastName").val("");
                    $("#TextBoxEmail").val("");
                    $("#TextBoxTitle").val("");
                    $("#TextBoxPhone").val("");
                    let validator = $("#EmployeeModalForm").validate();
                    validator.resetForm();
                    $("#modalstatus").attr("class", "");
                    let lastName = $("#TextBoxFindLastName").val();//
                    $("#myModal").modal("toggle");
                    $("#modalstatus").text("please wait...");
                    let response = await fetch(`api/employee/${lastName}`);//
                    if (!response.ok)
                        throw new Error(`Status = ${response.status}, Text - ${response.statusText}`);
                    let data = await response.json();
                    if (data.LastName !== "not found") {//
                        $("#TextBoxTitle").val(data.title);
                        $("#TextBoxFirstName").val(data.firstname);
                        $("#TextBoxLastName").val(data.lastname);
                        $("#TextBoxPhone").val(data.phoneno);
                        $("#TextBoxEmail").val(data.email);
                        $("#modalstatus").text("employee found");
                        sessionStorage.setItem("Id", data.Id);
                        sessionStorage.setItem("DepartmentId", data.departmentId);
                        sessionStorage.setItem("Timer", data.Timer);
                    } else {
                        $("#TextBoxTitle").val("not found");
                        $("#TextBoxFirstName").val("");
                        $("#TextBoxLastName").val("");
                        $("#TextBoxPhone").val("");
                        $("#TextBoxEmail").val("");
                        $("#modalstatus").text("no such employee");
                    }
                } catch (error) {
                    $("$status").text(error.message);
                }
    });
    //END OF VALIDATION


    //GET ALL
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding employee info...");
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let payload = await response.json();
                buildEmployeeList(payload);
                msg === "" ?
                    $("#status").text("Employees loaded.") : $("#status").text(`${msg} - Employees Loaded`);
            } else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("GETALLERR: no such path on server");
            }
            //new code
            response = await fetch(`api/department`);
            if (response.ok) {
                let depts = await response.json();
                sessionStorage.setItem("alldepartments", JSON.stringify(depts));
            }
            else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            }
            else {
                $('#status').text("GETALLDEPTERR: no such path on server")
            }//new code end
        } catch (error) {
            $("#status").text(error.message);
        }
    };

    //SETUP FOR UPDATE----------------------------------------------------------------------
    const setupForUpdate = (id, data) => {
        $("#deletebutton").show();
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");

        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {

                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstName").val(employee.firstname);
                $("#TextBoxLastName").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);
                $("#TextBoxDepartment").val(employee.departmentId);
                sessionStorage.setItem("id", employee.id);
                sessionStorage.setItem("departmentId", employee.departmentId);
                sessionStorage.setItem("timer", employee.timer);
                $("#modalstatus").text("update data");
                loadDepartmentDDL(employee.departmentId);
                $("#myModal").modal("toggle");
                $("#myModalLabel").text("Update");
            }
        });
    };

    //load department by id----------------------------------------------------------------------
    const loadDepartmentDDL = (empdepts) => {
        html = '';
        $('#ddlDepartments').empty();
        let allDepartments = JSON.parse(sessionStorage.getItem('alldepartments'));
        allDepartments.map(depts => html += `<option value="${depts.id}">${depts.name}</option>`);
        $('#ddlDepartments').append(html);
        $('#ddlDepartments').val(empdepts);
    };

    //SETUP ADD----------------------------------------------------------------------
    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#deletebutton").hide();
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
        $("#myModalLable").text("Add");
        clearModalFields();
    };

    //CLEAR MODAL FIELDS----------------------------------------------------------------------
    const clearModalFields = () => {
        loadDepartmentDDL(-1);
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstName").val("");
        $("#TextBoxLastName").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        $("#TextBoxDepartment").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId");
        sessionStorage.removeItem("timer");
        $("#myModal").modal("toggle");
        $("#EmployeeModalForm").validate().resetForm();
    };

    //ADD ----------------------------------------------------------------------
    const add = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstName").val();
            emp.lastname = $("#TextBoxLastName").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            emp.departmentId = parseInt($("#ddlDepartments").val());
            emp.id = -1;
            emp.timer = null;
            emp.staffPicture64 = null;

            let response = await fetch(`api/employee`, { //it's giving me a "Failed to load resource: Timer field is required" error here, but I can't figure out why. It's passing a timer, just a null one. I tried altering the field in HelpDeskEntity, but it didn't help
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            }
            else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("ADD ERR: no such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
        $("#myModal").modal("toggle");
    };

    //DELETE ----------------------------------------------------------------------
    const _delete = async () => {
        try {
            let response = await fetch(`api/employee/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=uft-8' }
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, problem on delete server side, see server console`);
            }
            $('#myModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    };//delete

    $('#deletebutton').click(() => {
        if (window.confirm('Are you sure?'))
            _delete();
    });//delete confirmation

    //UPDATE ----------------------------------------------------------------------
    const update = async () => {
        try {
            emp = new Object();
            emp.title = $("#TextBoxTitle").val();
            emp.firstname = $("#TextBoxFirstName").val();
            emp.lastname = $("#TextBoxLastName").val();
            emp.phoneno = $("#TextBoxPhone").val();
            emp.email = $("#TextBoxEmail").val();
            emp.id = parseInt(sessionStorage.getItem("id"));
            emp.departmentId = parseInt($("#ddlDepartments").val());//= parseInt(sessionStorage.getItem("departmentId"));
            emp.timer = sessionStorage.getItem("timer");
            emp.picture64 = null;

            let response = await fetch(`api/employee`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            }
            else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else {
                $("#status").text("UPDATEERR: no such path on server");
            }
        } catch (error) {
            $("#status").text(error.message);
        }
        $("#myModal").modal("toggle");
    };

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    //EMPLOYEE LIST BUTTON ----------------------------------------------------------------------
    $("#employeeList").click((e) => {
        // clearModalFields();
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === "employeeList" || id === "") {
            id = e.target.id;
        }
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allEmployees"));//employees may need to be Lowercase
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else { return false; }
    });

    //BUILD EMPLOYEE LIST ----------------------------------------------------------------------
    const buildEmployeeList = (data) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">Employee Info</div>
                   <div class="list-group-item row d-flex text-center" id="heading">
                       <div class="col-4 h4">Title</div>
                       <div class="col-4 h4">First</div>
                       <div class="col-4 h4">Last</div>
                   </div>`);
        div.appendTo($("#employeeList"));
        sessionStorage.setItem("allEmployees", JSON.stringify(data));
        btn = $(`<button class="list-group-item row d-flex" id="0">...click to add employee</button>`);
        btn.appendTo($("#employeeList"));
        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);
            btn.html(`<div class="col-4" id="employeetitle${emp.id}">${emp.title}</div>
                        <div class="col-4" id="employeefname${emp.id}">${emp.firstname}</div>
                        <div class="col-4" id="employeelname${emp.id}">${emp.lastname}</div>`);
            btn.appendTo($("#employeeList"));
        });
    };

    getAll("");


    //ERROR REPORT----------------------------------------------------------------------
    const errorRtn = (problemJson, status) => {
        if (status > 499) {
            $("#status").text("Problem server side, see debug console");
        }
        else {
            let keys = Object.keys(problemJson.errors)
            problem = {
                status: status,
                statusText: problemJson.errors[keys[0]][0],
            };
            $("#status").text("Problem client side, see browser console");
            console.log(problem);
        }
    }

});