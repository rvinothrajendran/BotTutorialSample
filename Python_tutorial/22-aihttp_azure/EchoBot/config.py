#!/usr/bin/env python3
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

import os

class DefaultConfig:
    """ Bot Configuration """

    PORT = 3978
    APP_ID = os.environ.get("MicrosoftAppId", "e1a03f01-6427-4c38-9a27-fd0e83fcea24")
    APP_PASSWORD = os.environ.get("MicrosoftAppPassword", "xNlTk3E2A8873_IB06jMCAQEOoo~pos-.k")
