define(['jquery', 'bootstrapValidator', 'moment', 'bootstrapDateTimePicker'], function ($) {
    var initValidator = function () {


        $(".course_form").bootstrapValidator({
            // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {

                Id: {
                    message: 'The Id is not valid',
                    validators: {
                        numeric: {

                        },
                        notEmpty: {

                        }
                    }
                },

                Title: {
                    message: 'The Title is not valid',
                    validators: {
                        max: {
                            max: 50
                        }
                    }
                },

                Credits: {
                    message: 'The Credits is not valid',
                    validators: {
                        numeric: {

                        },
                        notEmpty: {

                        }
                    }
                },

                
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();

            var $form = $(e.target);

            viewModel.Courses()[0].Course.DepartmentID = viewModel.selectedDepartment ? viewModel.selectedDepartment.Id : '';




            var jsonObj = {
                model: viewModel.Courses()[0]
            };

            $.ajax({
                type: "POST",
                url: $form.attr('action'),
                dataType: "json",
                data: JSON.stringify(jsonObj),
                contentType: "application/json; charset=utf-8",
                success: function (response) {
                    window.history.back();
                }
            });
        });
    };

    var initViewModel = function (model) {
        viewModel = model;
    };



    var formValidator = {
        initViewModel: initViewModel,
        initValidator: initValidator
    };


    return formValidator;
});
