(function ($) {

    var _admissionPeriodService = abp.services.app.admissionPeriod,
        l = abp.localization.getSource("MyProject"),
        _$modal = $("#AdmissionPeriodCreateModal"),
        _$form = _$modal.find("form"),
        _$table = $("#AdmissionPeriodsTable");

    var _$admissionPeriodsTable = _$table.DataTable({

        paging: true,
        serverSide: true,
        processing: true,

        listAction: {
            ajaxFunction: _admissionPeriodService.getAll,

            inputFilter: function () {
                return {};
            }
        },

        buttons: [
            {
                name: "refresh",
                text: '<i class="fas fa-redo-alt"></i>',
                action: function () {
                    _$admissionPeriodsTable.draw(false);
                }
            }
        ],

        responsive: {
            details: {
                type: "column"
            }
        },

        columnDefs: [

            {
                targets: 0,
                className: "control",
                defaultContent: "",
                orderable: false
            },

            {
                targets: 1,
                data: "name"
            },

            {
                targets: 2,
                data: "schoolYear"
            },

            {
                targets: 3,
                data: "startDate",
                render: function (data) {
                    if (!data) return "";

                    return moment(data).format("DD/MM/YYYY HH:mm");
                }
            },

            {
                targets: 4,
                data: "endDate",
                render: function (data) {
                    if (!data) return "";

                    return moment(data).format("DD/MM/YYYY HH:mm");
                }
            },

            {
                targets: 5,
                data: "isActive",
                orderable: false,

                render: function (data) {
                    return `
                        <input type="checkbox"
                               disabled
                               ${data ? "checked" : ""}>
                    `;
                }
            },

            {
                targets: 6,
                data: null,
                orderable: false,
                autoWidth: false,

                render: function (data, type, row) {

                    return [
                        `<button type="button"
                                 class="btn btn-sm bg-secondary edit-admission-period"
                                 data-admission-period-id="${row.id}"
                                 data-bs-toggle="modal"
                                 data-bs-target="#AdmissionPeriodEditModal">`,
                        `<i class="fas fa-pencil-alt"></i> Edit`,
                        `</button>`,

                        ` <button type="button"
                                  class="btn btn-sm bg-danger delete-admission-period"
                                  data-admission-period-id="${row.id}"
                                  data-name="${row.name}">`,
                        `<i class="fas fa-trash"></i> Delete`,
                        `</button>`
                    ].join("");
                }
            }
        ]
    });


    // =========================
    // CREATE
    // =========================

    _$form.on("submit", function (e) {

        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var admissionPeriod = _$form.serializeFormToObject();

        admissionPeriod.isActive =
            _$form.find("input[name='isActive']").is(":checked");

        console.log("DATA SEND:", admissionPeriod);

        abp.ui.setBusy(_$modal);

        _admissionPeriodService.create(admissionPeriod)

            .done(function (result) {

                console.log("CREATE SUCCESS:", result);

                _$modal.modal("hide");

                _$form[0].reset();

                abp.notify.success("Tạo đợt tuyển sinh thành công");

                _$admissionPeriodsTable.ajax.reload();

            })

            .fail(function (error) {

                console.error("CREATE ERROR:", error);

            })

            .always(function () {

                abp.ui.clearBusy(_$modal);

            });
    });


    // =========================
    // OPEN CREATE MODAL
    // =========================

    $(document).on(
        "click",
        'a[data-bs-target="#AdmissionPeriodCreateModal"]',
        function () {

            _$form[0].reset();

        }
    );


})(jQuery);