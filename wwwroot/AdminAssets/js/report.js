document.addEventListener("DOMContentLoaded", function () {
    const reportDropdown = document.getElementById("report-dropdown");
    const generateReportButton = document.getElementById("generate-report");
    const reportTitleInput = document.getElementById("ReportTitle");
    const pdfPathInput = document.getElementById("PdfPath");

    // Tables
    const annualReportTable = document.getElementById("an-report");
    const monthlyReportTable = document.getElementById("mn-report");
    const memberProgressTable = document.getElementById("mp-report");

    // Map table IDs to report types
    const tables = {
        annual: annualReportTable,
        monthly: monthlyReportTable,
        memberProgress: memberProgressTable
    };

    // Function to hide all tables
    function hideAllTables() {
        for (const tableId in tables) {
            const table = tables[tableId];
            if (table) {
                table.style.display = "none";
            }
        }
    }

    // Function to format today's date
    function getFormattedDate() {
        const today = new Date();
        const dd = String(today.getDate()).padStart(2, '0');
        const mm = String(today.getMonth() + 1).padStart(2, '0'); // Months are 0-based
        const yyyy = today.getFullYear();
        return `${yyyy}-${mm}-${dd}`; // Format: YYYY-MM-DD
    }

    // Function to initialize DataTables
    function initializeDataTable(table) {
        if ($.fn.DataTable.isDataTable(table)) {
            $(table).DataTable().destroy();
        }
        

        $(table).DataTable({
            destroy: true,
            dom: "Bfrtip",
            buttons: ["excel", "pdf"],
            paging: true, // Enable pagination
            searching: false, // Disable search bar
            order: [], // Disable initial column ordering
            columnDefs: [
                { targets: "_all", defaultContent: "N/A" } // Fallback for empty cells
            ]
        });
    }

    // Event listener for the Generate Report button
    generateReportButton.addEventListener("click", function () {
        const selectedReportType = reportDropdown.value;

        if (!selectedReportType) {
            alert("Please select a report type first.");
            return;
        }

        // Hide all tables
        hideAllTables();

        // Get the table for the selected report type
        const selectedTable = tables[selectedReportType];

        if (selectedTable) {
            // Display the selected table
            selectedTable.style.display = "table";

            // Initialize DataTable for the selected table
            initializeDataTable(selectedTable);

            // Automatically fill the ReportTitle field
            const formattedDate = getFormattedDate();
            reportTitleInput.value = `${selectedReportType} Report - ${formattedDate}`;

        }
    });
    reportDropdown.addEventListener("change", function () {
        // Clear form fields when dropdown value changes
        const selectedReportType = reportDropdown.value;

        if (selectedReportType) {
            const formattedDate = getFormattedDate();
            reportTitleInput.value = `${selectedReportType} Report - ${formattedDate}`;
        } else {
            reportTitleInput.value = "";
        }
    });
});
document.addEventListener("DOMContentLoaded", function () {
    const searchButton = $('#search-button'); // The Search button
    const startDateInput = $('#startDate'); // The Start Date input
    const endDateInput = $('#endDate'); // The End Date input
    const tableBody = $('table.table tbody'); // The table body where rows will be appended

    // Attach event listener to the Search button
    searchButton.on('click', function () {
        const startDate = startDateInput.val(); // Get Start Date
        const endDate = endDateInput.val(); // Get End Date

        // Validate input dates
        if (!startDate || !endDate) {
            alert('Please select both start and end dates.');
            return;
        }

        // Disable the button and show a loading state
        searchButton.prop('disabled', true).text('Loading...');

        // Perform AJAX request
        $.ajax({
            url: '/Home/SearchSubscriptions', // Adjust to match your controller action URL
            method: 'GET',
            data: { startDate: startDate, endDate: endDate },
            success: function (data) {
                console.log('Response Data:', data); // Debug: Log the response

                // Clear the table body
                tableBody.empty();

                if (data && data.length > 0) {
                    // Iterate over the data and append rows
                    data.forEach(function (item) {
                        const row = `
                            <tr>
                                <td>${item.subscriptionId || 'N/A'}</td>
                                <td>${item.memberId || 'N/A'}</td>
                                <td>${item.memberName || 'N/A'}</td>
                                <td>${item.planId || 'N/A'}</td>
                                <td>${item.price || 'N/A'}</td>
                                <td>${item.startDate || 'N/A'}</td>
                                <td>${item.endDate || 'N/A'}</td>
                            </tr>
                        `;
                        tableBody.append(row);
                    });
                } else {
                    // If no data is returned, show a message
                    const emptyRow = `
                        <tr>
                            <td colspan="7" class="text-center">No data available for the selected dates.</td>
                        </tr>
                    `;
                    tableBody.append(emptyRow);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error Details:', error); // Debug: Log error details
                alert('An error occurred while fetching data. Please try again.');
            },
            complete: function () {
                // Re-enable the button and reset its text
                searchButton.prop('disabled', false).text('Search');
            }
        });
    });
});
