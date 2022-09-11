$(function () {
    var l = abp.localization.getResource("EmailPostOffice");
	
	var mailQueueService = window.emailPostOffice.mailQueues.mailQueues;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "MailQueues/CreateModal",
        scriptUrl: "/Pages/MailQueues/createModal.js",
        modalClass: "mailQueueCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "MailQueues/EditModal",
        scriptUrl: "/Pages/MailQueues/editModal.js",
        modalClass: "mailQueueEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            recipient: $("#RecipientFilter").val(),
			recipientName: $("#RecipientNameFilter").val(),
			sender: $("#SenderFilter").val(),
			senderName: $("#SenderNameFilter").val(),
			subject: $("#SubjectFilter").val(),
			content: $("#ContentFilter").val(),
			retryMin: $("#RetryFilterMin").val(),
			retryMax: $("#RetryFilterMax").val(),
            success: (function () {
                var value = $("#SuccessFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
            suspend: (function () {
                var value = $("#SuspendFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })(),
			freezeTimeMin: $("#FreezeTimeFilterMin").data().datepicker.getFormattedDate('yyyy-mm-dd'),
			freezeTimeMax: $("#FreezeTimeFilterMax").data().datepicker.getFormattedDate('yyyy-mm-dd')
        };
    };

    var dataTable = $("#MailQueuesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: false,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(mailQueueService.getList, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('EmailPostOffice.MailQueues.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('EmailPostOffice.MailQueues.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    mailQueueService.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
			{ data: "recipient" },
			{ data: "recipientName" },
			{ data: "sender" },
			{ data: "senderName" },
			{ data: "subject" },
			{ data: "content" },
			{ data: "retry" },
            {
                data: "success",
                render: function (success) {
                    return success ? l("Yes") : l("No");
                }
            },
            {
                data: "suspend",
                render: function (suspend) {
                    return suspend ? l("Yes") : l("No");
                }
            },
            {
                data: "freezeTime",
                render: function (freezeTime) {
                    if (!freezeTime) {
                        return "";
                    }
                    
					var date = Date.parse(freezeTime);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            }
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $("#NewMailQueueButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reload();
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reload();
    });
    
    
});
