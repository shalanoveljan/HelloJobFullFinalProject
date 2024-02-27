
function MyFunction() {
    debugger
     
        $.ajax({
            type: 'GET',
            url: 'https://localhost:7066/Course/SortCourses/' + getCurrentOptionId(),
            success: function (data) {

            }

        })


}
function getCurrentOptionId() {
    debugger
    var selectElement = document.getElementById("sort");
    var selectedIndex = selectElement.selectedIndex;
    var selectedOption = selectElement.options[selectedIndex];
    var selectedOptionId = selectedOption.value;
    console.log(selectedOptionId);
    debugger
    return selectedOptionId;
}
