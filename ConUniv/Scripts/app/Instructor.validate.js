define(['jquery', 'bootstrapValidator', 'moment', 'bootstrapDateTimePicker'], function ($) {
    var initValidator = function () {
        $('#EnrollmentDate').datetimepicker({
            pickTime: false,
            useMinutes: false,               
            useSeconds: false,               
            useCurrent: false
        });

        $(".instructor_form").bootstrapValidator({
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

                InstructorID: {
                     message: 'The InstructorID is not valid',
                     validators: {
                        numeric: {
                            
                          },
notEmpty: {
                            
                          }
                     }
                   },

                Location: {
                     message: 'The Location is not valid',
                     validators: {
                        max: {
                            max: 50
                          }
                     }
                   },
                Id: {
                     message: 'The Id is not valid',
                     validators: {
                        numeric: {
                            
                          },
notEmpty: {
                            
                          }
                     }
                   },

                LastName: {
                     message: 'The LastName is not valid',
                     validators: {
                        notEmpty: {
                            max: 50
                          },
max: {
                            
                          }
                     }
                   },

                FirstName: {
                     message: 'The FirstName is not valid',
                     validators: {
                        max: {
                            max: 50
                          },
notEmpty: {
                            
                          }
                     }
                   },

                HireDate: {
                     message: 'The HireDate is not valid',
                     validators: {
                        date: {
                            
                          }
                     }
                   },

                EnrollmentDate: {
                     message: 'The EnrollmentDate is not valid',
                     validators: {
                        date: {
                            
                          }
                     }
                   },

                Discriminator: {
                     message: 'The Discriminator is not valid',
                     validators: {
                        max: {
                            max: 128
                          },
notEmpty: {
                            
                          }
                     }
                   },
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();

            var $form = $(e.target);

            

            viewModel.Instructors()[0].Instructor.HireDate = viewModel.dateSelected();

            var jsonObj = {
                model: viewModel.Instructors()[0]
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

    $('#EnrollmentDate')
        .on('dp.change dp.show', function (e) {
            // Revalidate the date when user change it
            $('.user_form').bootstrapValidator('revalidateField', 'EnrollmentDate');
        });
    
    var formValidator = {        
        initViewModel: initViewModel,
        initValidator: initValidator
    };
    

    return formValidator;
});
