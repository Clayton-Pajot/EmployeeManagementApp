$(function () {
    $("#getbutton").click(async (e) => {
        try {
            let email = $("#TextBoxEmail").val();
            $("#status").text("please wait...");
            let response = await fetch(`/api/employee/${email}`);
            if (response.ok) {
                let data = await response.json();
                if (data.email !== "not found") {

                    $("#email").text(data.email);
                    $("#title").text(data.title);
                    $("#firstname").text(data.firstname);
                    $("#lastname").text(data.lastname);
                    $("#phone").text(data.phoneno);
                    $("#status").text("employee  found");
                }
                else {
                    $("#email").text("not found");
                    $("#title").text("");
                    $("#firstname").text("");
                    $("#lastname").text("");
                    $("#phone").text("");
                    $("#status").text("no employee  found");
                }
            }
            else if (response.status !== 404) {
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            }
            else {
                $("#status").text("no such path on server");
            }
        }
        catch (error) {1
            $("#status").text(error.message);
        }//try catch

    });//click event

});//jQuery ready method


//server reached, but had problem with call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem Server Side, see debug console");
    }
    else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0],
        };
        $("#status").text("Problem Client Side, see browser console");
        console.log(problem);
    }
}