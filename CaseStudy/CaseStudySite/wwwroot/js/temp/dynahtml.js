


$(function () { // dynahtml.js
    const buildDynamicList = () => {
        $("#dynaList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status"> Sample Bootstrap Styling</div>
            <div class="list-group-item row d-flex text-center" id="heading">
                <div class="col-4 h4">Column 1</div>
                <div class="col-4 h4">Column 2</div>
                <div class="col-4 h4">Column 3</div>
            </div>`);
        div.appendTo($("#dynaList"));
        let data = [
            { "col1": "Some", "col2": "student", "col3": "data" },
            { "col1": "Will", "col2": "be", "col3": "listed" },
            { "col1": "in", "col2": "these", "col3": "columns" },
            { "col1": "Try", "col2": "clicking", "col3": "one" }
        ];
        data.map(row => {
            btn = $(`<button class="list-group-item row d-flex p2" onclick="confirm('this will eventually be a modal');">`);
            btn.html(`<div class="col-4">${row.col1}</div>
                    <div class="col-4">${row.col2}</div>
                    <div class="col-4">${row.col3}</div>`
            );
            btn.appendTo($("#dynaList"));
        });
    };
    buildDynamicList();
});