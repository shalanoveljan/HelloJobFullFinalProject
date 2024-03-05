﻿
let similar_Resume = document.getElementById("similar-resume");

function MyFunction() {
    const id = getCurrentOptionId();
    const dto = getFilterDto(); 

    console.log(dto)

    $.ajax({
        type: 'Post',
        url: `https://localhost:7066/Resume/SortResumes/${id}`,
        data: dto,
        success: function (data) {
            similar_Resume.innerHTML = data;
        }
    });
}


function getCurrentOptionId() {
    var selectElement = document.getElementById("sort");
    var selectedIndex = selectElement.selectedIndex;
    var selectedOption = selectElement.options[selectedIndex];
    var selectedOptionId = selectedOption.value;
    return selectedOptionId;
}
$(document).ready(function () {
    $("input[name='CategoriesIds'], input[name='LanguagesIds'],input[name='EducationsIds'], #certificate_1, input[name='Mode'],input[name='Gender'],input[name='Status'], #slider-1, #slider-2,#silder-3,#slider-4").change(function (e) {
        e.preventDefault();
        var dto = getFilterDto(); 
        $.ajax({
            url: '/Resume/FilterResumes',
            type: 'POST',
            data: {
                dto: dto
            },
            success: function (data) {
                similar_Resume.innerHTML = data;
                var ResumeCountValue = $("#ResumeCount").val();
                $('#count_items_head').text(ResumeCountValue);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});

function getFilterDto() {
    var categoriesIds = [];
    var languagesIds = [];
    var educationsIds = [];
    $("input[name='CategoriesIds']:checked").each(function () {
        categoriesIds.push(parseInt($(this).val()));
    });
    $("input[name='LanguagesIds']:checked").each(function () {
        languagesIds.push(parseInt($(this).val()));
    });
    $("input[name='EducationsIds']:checked").each(function () {
        educationsIds.push(parseInt($(this).val()));
    });
    var isDriverLicense = $("#certificate_1").is(":checked");
    var JobMode = [];
    var status = [];
    var gender = [];
    $("input[name='Mode']:checked").each(function () {
        JobMode.push(parseInt($(this).val()));
    });
    $("input[name='Gender']:checked").each(function () {
        gender.push(parseInt($(this).val()));
    });

    $("input[name='Status']:checked").each(function () {
        status.push(parseInt($(this).val()));
    });


    var minExperience = parseInt($("#slider-1").val());
    var maxExperience = parseInt($("#slider-2").val());

    var minSalary = parseInt($("#slider-3").val());
    var maxSalary = parseInt($("#slider-4").val());

    return {
        EducationsIds: educationsIds,
        LanguagesIds: languagesIds,
        CategoriesIds: categoriesIds,
        IsDriverLicense: isDriverLicense,
        Job_Mode: JobMode,
        MaritalStatus: status,
        Gender: gender,
        MinSalary: minSalary,
        MaxSalary: maxSalary,
        MinExperience: minExperience,
        MaxExperience: maxExperience,
    };
}

