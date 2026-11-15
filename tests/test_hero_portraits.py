import unittest

from scripts.sync_hero_portraits import check_portraits


class PortraitSyncTests(unittest.TestCase):
    def test_all_portraits_match_unity_resources(self):
        self.assertEqual(check_portraits(), [])


if __name__ == "__main__":
    unittest.main()

